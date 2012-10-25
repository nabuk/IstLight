using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Synchronization;
using IstLight.Domain.Extensions;

namespace IstLight.Domain.UnitTests.Synchronization
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
