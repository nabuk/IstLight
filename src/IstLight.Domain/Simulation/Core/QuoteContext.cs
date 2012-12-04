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
