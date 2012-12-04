// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;

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
