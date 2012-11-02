using System;
using IstLight.Strategy;
using IstLight.Synchronization;

namespace IstLight.Simulation.Core
{
    public class QuoteContext : IQuoteContext
    {
        private readonly SyncTickers syncTickers;
        private int currentObservationIndex;

        public QuoteContext(SyncTickers syncTickers)
        {
            this.syncTickers = syncTickers;
            this.currentObservationIndex = -1;
        }

        public int CurrentObservationIndex { get { return currentObservationIndex; } }
        public void IncrementObservationIndex() { currentObservationIndex++; }

        #region IQuoteContext
        public DateTime Date
        {
            get
            {
                return syncTickers[CurrentObservationIndex].Date;
            }
        }

        public int TickerCount
        {
            get { return syncTickers.TickerCount; }
        }

        public IReadOnlyList<ISimpleTickerQuote> GetQuotes(int tickerIndex)
        {
            return syncTickers.SimplifiedTickers[tickerIndex]
                .Take(syncTickers[CurrentObservationIndex].CurrentQuoteCount[tickerIndex])
                .Reverse();
        }

        public int? GetTickerIndex(string tickerName)
        {
            return syncTickers.TickerIndexByName(tickerName);
        }

        public TickerDescription GetTickerDescription(int tickerIndex)
        {
            return syncTickers.Descriptions[tickerIndex];
        }

        public TimeSpan? Span
        {
            get
            {
                return currentObservationIndex < 1 ?
                    null
                    :
                    (TimeSpan?)(syncTickers[currentObservationIndex].Date-syncTickers[currentObservationIndex-1].Date);
            }
        }

        public int GetNewQuoteCount(int tickerIndex)
        {
            int currentCount = currentObservationIndex < 0 ? 0 : syncTickers[currentObservationIndex].CurrentQuoteCount[tickerIndex];
            int prevCount = currentObservationIndex < 1 ? 0 : syncTickers[currentObservationIndex-1].CurrentQuoteCount[tickerIndex];

            return currentCount - prevCount;
        }

        public bool IsLast
        {
            get { return currentObservationIndex == syncTickers.Count - 1; }
        }

        public bool IsFirst
        {
            get { return currentObservationIndex == 0; }
        }
        #endregion IQuoteContext
    }
}
