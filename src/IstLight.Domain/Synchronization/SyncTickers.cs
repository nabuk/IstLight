using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Extensions;

namespace IstLight.Synchronization
{
    public class SyncTickers : MultiQuoteList<Observation>
    {
        public SyncTickers(
            IReadOnlyList<SimplifiedTicker> simplifiedTickers,
            IReadOnlyList<Observation> observations,
            IReadOnlyList<TickerDescription> descriptions) : base(observations, descriptions)
        {
            if (simplifiedTickers == null) throw new ArgumentNullException("simplifiedTickers");
            if (simplifiedTickers.Count != descriptions.Count ||
                observations.Any(o => o.CurrentQuoteCount.Count != simplifiedTickers.Count))
                throw new ArgumentException("Ticker count does not match.");

            this.SimplifiedTickers = simplifiedTickers;
        }

        public IReadOnlyList<SimplifiedTicker> SimplifiedTickers { get; private set; }
    }

    public static class SyncTickersExtensions
    {
        public static ISimpleTickerQuote LastObservedQuote(
            this SyncTickers syncTickers,
            int tickerIndex,
            int observationIndex)
        {
            if (tickerIndex < 0 || tickerIndex >= syncTickers.TickerCount) throw new ArgumentException("Wrong ticker index.");
            if (observationIndex < 0 || observationIndex >= syncTickers.Count) throw new ArgumentException("Wrong observation index.");

            int currentQuoteCount = syncTickers[observationIndex].CurrentQuoteCount[tickerIndex];
            return currentQuoteCount == 0 ? null : syncTickers.SimplifiedTickers[tickerIndex][currentQuoteCount - 1];
        }

        public static IReadOnlyList<ISimpleTickerQuote> ObservedQuotes(
            this SyncTickers syncTickers,
            int tickerIndex,
            int observationIndex)
        {
            if (tickerIndex < 0 || tickerIndex >= syncTickers.TickerCount) throw new ArgumentException("Wrong ticker index.");
            if (observationIndex < 0 || observationIndex >= syncTickers.Count) throw new ArgumentException("Wrong observation index.");

            int currentCount = syncTickers[observationIndex].CurrentQuoteCount[tickerIndex];
            var ticker = syncTickers.SimplifiedTickers[tickerIndex];
            return new ProxiedReadOnlyList<ISimpleTickerQuote>(i => ticker[i], () => currentCount);
        }
    }
}
