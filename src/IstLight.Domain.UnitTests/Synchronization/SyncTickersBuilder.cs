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
using System.Collections.Generic;
using System.Linq;
using IstLight.Synchronization;

namespace IstLight.UnitTests.Synchronization
{
    public class SyncTickersBuilder
    {
        private readonly DateTime firstDate;
        private readonly TimeSpan referenceSpan;

        private readonly List<SimplifiedTicker> tickers = new List<SimplifiedTicker>();
        private readonly List<Observation> observations = new List<Observation>();
        private readonly List<TickerDescription> descriptions = new List<TickerDescription>();

        public SyncTickersBuilder(DateTime? referenceDate = null, TimeSpan? referenceSpan = null)
        {
            this.firstDate = referenceDate ?? new DateTime(2000, 1, 1);
            this.referenceSpan = referenceSpan ?? TimeSpan.FromDays(1);
        }

        public SyncTickers Build()
        {
            return new SyncTickers(
                tickers.AsReadOnlyList(),
                observations.AsReadOnlyList(),
                descriptions.AsReadOnlyList());
        }

        public IReadOnlyList<SimplifiedTicker> Tickers { get { return tickers.AsReadOnlyList(); } }
        public IReadOnlyList<Observation> Observations { get { return observations.AsReadOnlyList(); } }
        public IReadOnlyList<TickerDescription> Descriptions { get { return descriptions.AsReadOnlyList(); } }

        #region Ticker
        public SyncTickersBuilder AddTickerFromQuotes(params ISimpleTickerQuote[] quotes)
        {
            this.tickers.Add(new SimplifiedTicker(Ext.ROL(quotes)));
            return this;
        }

        public SyncTickersBuilder AddTicker(params int[] offsets)
        {
            return AddTickerFromQuotes(
                offsets
                .Select((s, i) => new SimpleTickerQuote(firstDate + referenceSpan.Mult(s), 1, 1))
                .ToArray());
        }
        #endregion

        #region Observation
        public SyncTickersBuilder AddObservations(params Observation[] observations)
        {
            this.observations.AddRange(observations);
            return this;
        }

        public SyncTickersBuilder AddObservation(int offset, params int[] currentQuoteCount)
        {
            this.observations.Add(new Observation(currentQuoteCount.AsReadOnlyList(), firstDate + referenceSpan.Mult(offset)));
            return this;
        }
        #endregion

        #region Description
        public SyncTickersBuilder AddDescription(string name = null, bool buyable = true)
        {
            descriptions.Add(new TickerDescription { Name = name ?? Guid.NewGuid().ToString(), CanBeBought = buyable });
            return this;
        }

        public SyncTickersBuilder AddDescriptions(int count)
        {
            descriptions.AddRange(
            Enumerable.Range(0, count)
                .Select(i => new TickerDescription { Name = Guid.NewGuid().ToString(), CanBeBought = true }));
            
            return this;
        }
        #endregion
    }
}
