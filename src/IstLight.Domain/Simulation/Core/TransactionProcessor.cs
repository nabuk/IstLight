using System;
using System.Collections.Generic;
using System.Linq;
using IstLight.Settings;
using IstLight.Strategy;

namespace IstLight.Simulation.Core
{
    public class TransactionProcessor : ITransactionProcessor
    {
        private readonly IQuoteContext quoteContext;
        private readonly Account account;

        private readonly TradeDelaySetting delay;
        private readonly CommissionSetting commission;

        private List<TransactionRequest> transactionRequests = new List<TransactionRequest>();
        private List<Transaction> madeTransactions = new List<Transaction>();
        private readonly double[] avgPrice;

        public TransactionProcessor(
            IQuoteContext quoteContext,
            Account account,
            ISimulationSettings settings)
        {
            this.quoteContext = quoteContext;
            this.account = account;

            this.delay = settings.Get<TradeDelaySetting>();
            this.commission = settings.Get<CommissionSetting>();

            this.avgPrice = new double[quoteContext.TickerCount];
        }

        bool ITransactionProcessor.AddRequest(int tickerIndex, double quantity)
        {
            if (!quoteContext.GetTickerDescription(tickerIndex).CanBeBought) return false;

            var req = new TransactionRequest(tickerIndex, quantity < 0 ? 0 : quantity, delay.Value);
            if (delay.Value == 0)
                return ProcessTransactionRequest(req);
            else
                transactionRequests.Add(req);

            return true;
        }

        private bool ProcessTransactionRequest(TransactionRequest req)
        {
            double quantityChange;
            double? tickerPrice;

            if (!quoteContext.GetTickerDescription(req.TickerIndex).CanBeBought) return false;
            if ((quantityChange = req.NewQuantity - account.GetTickerQuantity(req.TickerIndex)) == 0) return false;
            if ((tickerPrice = quoteContext.GetCurrentPrice(req.TickerIndex)) == null) return false;

            //Buy
            if (quantityChange > 0)
            {
                if ((quantityChange = Math.Min(quantityChange, commission.MaxQuantity(tickerPrice.Value, account.Cash))) <= 0) return false;
                MakeTransaction(req.TickerIndex, tickerPrice.Value, quantityChange);
            }
            //Sell
            else
            {
                if (Math.Abs(tickerPrice.Value * quantityChange) + account.Cash < commission.ComputeFee(quantityChange, tickerPrice.Value)) return false;
                MakeTransaction(req.TickerIndex, tickerPrice.Value, quantityChange);
            }

            return true;
        }

        private void MakeTransaction(int tickerIndex, double tickerPrice, double quantityChange)
        {
            double fee = commission.ComputeFee(quantityChange, tickerPrice);
            double transactionValue = (quantityChange * tickerPrice) + fee;
            TransactionType tType = quantityChange > 0 ? TransactionType.Buy : TransactionType.Sell;

            double quantity = account.GetTickerQuantity(tickerIndex);

            if (tType == TransactionType.Buy)
                avgPrice[tickerIndex] =
                    ((avgPrice[tickerIndex] * quantity) + transactionValue) / (quantity + quantityChange);

            double netProfit =
                tType == TransactionType.Sell ?
                    (transactionValue / quantityChange) / avgPrice[tickerIndex] : 0;

            madeTransactions.Add(new Transaction(tType, tickerIndex, quantityChange, netProfit, fee, transactionValue));

            account.ChangeTickerQuantity(tickerIndex, quantityChange, transactionValue * (-1));
        }

        public IReadOnlyList<Transaction> PopMadeTransactions()
        {
            var result = madeTransactions.ToArray();
            madeTransactions.Clear();
            return result.AsReadOnlyList();
        }

        public void RecalculateTransactionsDelay()
        {
            var toDecrementDelay = Enumerable.Range(0,quoteContext.TickerCount)
                .Select(tI => new { tI, newQuoteCount = quoteContext.GetNewQuoteCount(tI) })
                .Where(x => x.newQuoteCount > 0)
                .ToDictionary(x => x.tI, x => x.newQuoteCount);

            foreach (var req in transactionRequests.Where(req => toDecrementDelay.ContainsKey(req.TickerIndex)).ToArray())
                req.DecrementDelay();
        }

        public void ProcessPending()
        {
            foreach (var req in transactionRequests.Where(req => req.Delay == 0).ToArray())
            {
                ProcessTransactionRequest(req);
                transactionRequests.Remove(req);
            }
        }

        public void CloseAll()
        {
            foreach (int tI in Enumerable.Range(0, quoteContext.TickerCount)
                                .Where(tI => account.GetTickerQuantity(tI) > 0))
                ProcessTransactionRequest(new TransactionRequest(tI, 0, 0));
        }
    }
}
