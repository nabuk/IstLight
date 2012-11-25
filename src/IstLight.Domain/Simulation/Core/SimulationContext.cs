using System;
using System.Collections.Generic;
using System.Linq;
using IstLight.Settings;
using IstLight.Strategy;
using IstLight.Synchronization;

namespace IstLight.Simulation.Core
{
    public class SimulationContext : ISimulationContext
    {
        private ISimulationSettings settings;
        private QuoteContext quoteContext;
        private Account account;
        private TransactionProcessor transactionProcessor;
        private WalletContext walletContext;
        private StrategyBase strategy;
        private SyncTickers syncTickers;
        private List<SimulationResultQuote> resultQuotes;

        private double lastInterest;

        #region ISimulationContext

        bool ISimulationContext.Initialize(SyncTickers syncTickers, StrategyBase strategy, ISimulationSettings settings)
        {
            this.strategy = strategy;
            this.settings = settings;
            this.syncTickers = syncTickers;
            this.account = new Account(settings.Get<InitialEquitySetting>().Value);
            this.strategy.QuoteContext = this.quoteContext = new QuoteContext(syncTickers);
            this.transactionProcessor = new TransactionProcessor(quoteContext, account, settings);
            this.strategy.WalletContext = this.walletContext = new WalletContext(quoteContext, account, transactionProcessor);

            this.resultQuotes = new List<SimulationResultQuote>();

            return strategy.Initialize();
        }

        void ISimulationContext.BeginStep()
        {
            quoteContext.IncrementObservationIndex();
            transactionProcessor.RecalculateTransactionsDelay();
        }

        void ISimulationContext.EndStep()
        {
            if (quoteContext.IsLast && settings.Get<CloseAllOnLastBarSetting>().Value) transactionProcessor.CloseAll();

            var quantities = Enumerable.Range(0, quoteContext.TickerCount).Select(i => walletContext.GetQuantity(i)).AsReadOnlyList();
            var observation = syncTickers[quoteContext.CurrentObservationIndex];
            var cash = walletContext.Cash;
            resultQuotes.Add(new SimulationResultQuote(quantities, cash, observation)
            {
                Transactions = transactionProcessor.PopMadeTransactions(),
                Interest = lastInterest
            });
        }

        bool ISimulationContext.RunStrategy()
        {
            return strategy.Run();
        }

        void ISimulationContext.ProcessPendingTransactions()
        {
            transactionProcessor.ProcessPending();
        }

        void ISimulationContext.ApplyInterestRate()
        {
            lastInterest = account.ApplyInterestRate(settings.Get<AnnualInterestRateSetting>().Value, quoteContext.Span.Value);
        }

        SimulationResult ISimulationContext.GetResult()
        {
            return new SimulationResult(resultQuotes.AsReadOnlyList(), syncTickers) { Settings = settings };
        }

        string ISimulationContext.GetLastError()
        {
            return strategy.LastError;
        }

        void IDisposable.Dispose()
        {
            strategy.Dispose();
        }

        #endregion //ISimulationContext
    }
}