
using System;
using IstLight.Synchronization;
namespace IstLight.Simulation
{
    public class Transaction
    {
        public Transaction(TransactionType type, int tickerIndex, double quantity, double netProfit, double commission, double total)
        {
            this.Type = type;
            this.TickerIndex = tickerIndex;
            this.Quantity = quantity;
            this.NetProfit = netProfit;
            this.Commission = commission;
            this.Total = Math.Abs(total);
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

        public double Commission { get; private set; }

        public double Total { get; private set; }
    }
}
