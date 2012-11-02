using IstLight.Strategy;

namespace IstLight.Simulation.Core
{
    public class WalletContext : IWalletContext
    {
        private readonly IQuoteContext quoteContext;
        private readonly IAccount account;
        private readonly ITransactionProcessor transactionProcessor;

        public WalletContext(
            IQuoteContext quoteContext,
            IAccount account,
            ITransactionProcessor transactionProcessor)
        {
            this.quoteContext = quoteContext;
            this.account = account;
            this.transactionProcessor = transactionProcessor;
        }

        #region IWalletContext
        public double Cash
        {
            get
            {
                return account.Cash;
            }
        }

        public double GetQuantity(int tickerIndex)
        {
            return account.GetTickerQuantity(tickerIndex);
        }

        public bool SetQuantity(int tickerIndex, double quantity)
        {
            return transactionProcessor.AddRequest(tickerIndex, quantity);
        }
        #endregion //IWalletContext
    }
}