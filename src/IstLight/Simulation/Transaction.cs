
namespace IstLight.Simulation
{
    public class Transaction
    {
        public Transaction(TransactionType type, int tickerIndex, double quantity, double netProfit)
        {
            this.Type = type;
            this.TickerIndex = tickerIndex;
            this.Quantity = quantity;
            this.NetProfit = netProfit;
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
        public double NetProfit { get; private set; }
    }
}
