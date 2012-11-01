using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Synchronization;
using IstLight.Extensions;

namespace IstLight.Simulation
{
    public class SimulationResultQuote : IDate
    {
        private IReadOnlyList<Transaction> transactions;

        public SimulationResultQuote(
            IReadOnlyList<double> tickerQuantity,
            double cash,
            Observation observation)
        {
            this.TickerQuantity = tickerQuantity;
            this.Cash = cash;
            this.Observation = observation;
        }

        public IReadOnlyList<double> TickerQuantity { get; private set; }
        public double Cash { get; private set; }
        public Observation Observation { get; private set; }
        public IReadOnlyList<Transaction> Transactions
        {
            get
            {
                return transactions ?? new Transaction[] { }.AsReadOnlyList();
            }
            set
            {
                transactions = value;
            }
        }
        public DateTime Date
        {
            get { return Observation.Date; }
        }
    }

    public static class SimulationResultQuoteExtensions
    {
        public static double Equity(this SimulationResultQuote quote, SyncTickers syncTickers)
        {
            return quote.Cash + Enumerable.Range(0, syncTickers.TickerCount).Select(iT => quote.TickerEquity(iT, syncTickers)).Sum();
        }

        public static double TickerEquity(this SimulationResultQuote quote, int tickerIndex, SyncTickers syncTickers)
        {
            return quote.TickerQuantity[tickerIndex] == 0 ?
                0
                :
                quote.TickerQuantity[tickerIndex]
                    * syncTickers.SimplifiedTickers[tickerIndex][quote.Observation.CurrentQuoteCount[tickerIndex] - 1].Value;
        }

        public static double ExpositionToTicker(this SimulationResultQuote quote, int tickerIndex, SyncTickers syncTickers)
        {
            return quote.TickerEquity(tickerIndex, syncTickers) / quote.Equity(syncTickers);
        }
    }
}
