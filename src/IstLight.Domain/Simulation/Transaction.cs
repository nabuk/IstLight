
using System;
using IstLight.Synchronization;
namespace IstLight.Simulation
{
    public class Transaction
    {
        public Transaction(TransactionType type, int tickerIndex, double quantity, double netProfitRate, double commission, double cashFlow)
        {
            this.Type = type;
            this.TickerIndex = tickerIndex;
            this.Quantity = quantity;
            this.NetProfitRate = netProfitRate;
            this.Commission = commission;
            this.CashFlow = cashFlow;
        }

        /// <summary>
        /// Transaction type description flags.
        /// </summary>
        public TransactionType Type { get; private set; }

        /// <summary>
        /// Ticker index associated with this transaction.
        /// </summary>
        public int TickerIndex { get; private set; }

        public double Quantity { get; private set; }

        /// <summary>
        /// Transaction level profit (sell only).
        /// </summary>
        public double NetProfitRate { get; private set; }

        public double Commission { get; private set; }

        public double CashFlow { get; private set; }

        public double NetProfit
        {
            get
            {
                if (NetProfitRate == 0) return 0;
                return (CashFlow / (1 + NetProfitRate)) * NetProfitRate;
            }
        }
    }
}
