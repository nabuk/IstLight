using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain;
using IstLight.Domain.Settings;
using IstLight.Domain.Extensions;

namespace IstLight.Domain.Synchronization
{
    public static class SyncTickersFactory
    {
        private class MultiTickerSynchronizer
        {
            private readonly IReadOnlyList<Ticker> tickers;
            private readonly ISimulationSettings settings;

            private readonly TimeSpan stepSpan;
            private readonly DateTime from;
            private readonly DateTime to;

            private DateTime Min(params DateTime[] dates) { return dates.Min(); }
            private DateTime Max(params DateTime[] dates) { return dates.Max(); }

            public MultiTickerSynchronizer(IReadOnlyList<Ticker> tickers, ISimulationSettings settings)
            {
                this.tickers = tickers;
                this.settings = settings;

                this.stepSpan = settings.Get<PeriodSetting>().Type.ToTimeSpan();

                var range = settings.Get<SimulationRangeSetting>();
                if (range.Type == SimulationRangeType.Common)
                {
                    this.from = tickers.Select(t => t.From).Max();
                    this.to = tickers.Select(t => t.To).Min();
                }
                else
                {
                    this.from = Max(range.From.Date, tickers.Select(t => t.From).Min());
                    this.to = range.To.Date + (TimeSpan.FromDays(1) - TimeSpan.FromTicks(1));
                }

                //currentDate = this.from;
            }

            private int[] UpToDate(DateTime date, IReadOnlyList<int> currectQuoteCount)
            {
                int[] result = new int[tickers.Count];
                for (int iT = 0; iT < tickers.Count; iT++)
                {
                    int iQCount = currectQuoteCount[iT];
                    for (; iQCount < tickers[iT].Count; iQCount++)
                        if (tickers[iT][iQCount].Date > date)
                            break;

                    result[iT] = iQCount;
                }

                return result;
            }

            private Observation NextValidObservation(Observation observation)
            {
                DateTime nextObservationDate;

                if (observation == null)
                {
                    observation = new Observation(new int[tickers.Count].AsReadOnlyList(), from);
                    nextObservationDate = from;
                }
                else
                    nextObservationDate = Min(observation.Date + stepSpan, to);

                int[] nextQuoteCount = UpToDate(nextObservationDate, observation.CurrentQuoteCount);

                if (nextQuoteCount.SequenceEqual(observation.CurrentQuoteCount))
                {
                    var nextDates =
                        nextQuoteCount
                            .Select((iQCount, iT) => iQCount < tickers[iT].Count ? (DateTime?)tickers[iT][iQCount].Date : null)
                            .Where(d => d != null)
                            .ToArray();

                    if (nextDates.Length == 0 || (nextObservationDate = nextDates.Min().Value) > to)
                        return null;

                    nextQuoteCount = UpToDate(nextObservationDate, observation.CurrentQuoteCount);
                }

                return new Observation(nextQuoteCount.AsReadOnlyList(), nextObservationDate);
            }

            private IReadOnlyList<Observation> MakeObservations()
            {
                Observation observation = null;
                var result = new List<Observation>();

                while ((observation = NextValidObservation(observation)) != null)
                    result.Add(observation);
                
                #region Fix last date
                //if last observation date is later than last observed ticker quote date, then use last observed ticker quote date

                var lastObservation = result[result.Count - 1];

                DateTime maxObservedTickerDate =
                    lastObservation.CurrentQuoteCount
                    .Select((iC, iT) => new { iQ = iC-1, iT })
                    .Where(x => x.iQ >= 0)
                    .Max(x => tickers[x.iT][x.iQ].Date);

                if (lastObservation.Date > maxObservedTickerDate)
                    result[result.Count - 1] = new Observation(lastObservation.CurrentQuoteCount, Max(maxObservedTickerDate, from));
                #endregion

                return result.AsReadOnlyList();
            }

            private IReadOnlyList<Observation> TranslateToRecent(IReadOnlyList<Observation> observations)
            {
                var result = new List<Observation>();

                result.Add(new Observation(
                                observations[0].CurrentQuoteCount.Select(x => x > 0 ? 1 : 0).AsReadOnlyList(),
                                observations[0].Date));
                for (int iO = 1; iO < observations.Count; iO++)
                {
                    var current = observations[iO];
                    var previous = observations[iO - 1];
                    var previousResult = result[iO - 1];

                    result.Add(
                        new Observation(
                            current.CurrentQuoteCount.Zip(previous.CurrentQuoteCount, (x, y) => x != y ? 1 : 0)
                                .Zip(previousResult.CurrentQuoteCount, (x, y) => x + y).AsReadOnlyList(),
                            current.Date));
                }

                return result.AsReadOnlyList();
            }

            private IReadOnlyList<SimplifiedTicker> ExtractTickers(IReadOnlyList<Observation> observations)
            {
                var priceType = settings.Get<SimulationPriceSetting>().Type;
                var lastObservation = observations[observations.Count-1];

                return tickers.Select((ticker,iT) =>
                        ticker.Take(lastObservation.CurrentQuoteCount[iT]).Select(q => q.Simplify(priceType)))
                        .Select(quotes => new SimplifiedTicker(quotes.AsReadOnlyList()))
                        .AsReadOnlyList();
            }

            private IReadOnlyList<SimplifiedTicker> ExtractRecentTickers(IReadOnlyList<Observation> observations)
            {
                var priceType = settings.Get<SimulationPriceSetting>().Type;

                return tickers.Select((ticker,iT) =>
                        new SimplifiedTicker(
                            observations
                                .Select(o => o.CurrentQuoteCount[iT]-1)
                                .SkipWhile(x => x < 0)
                                .Distinct()
                                .Select(iQ => ticker[iQ].Simplify(priceType))
                                .AsReadOnlyList()))
                        .AsReadOnlyList();
            }

            private IReadOnlyList<TickerDescription> ExtractDescriptions()
            {
                return tickers.Select(t => t.GetDescription()).AsReadOnlyList();
            }
            
            public SyncTickers Sync()
            {
                bool onlyRecent = settings.Get<OnlyRecentQuotesSetting>().Value;
                
                var observations = MakeObservations();

                return onlyRecent ?
                    new SyncTickers(ExtractRecentTickers(observations), TranslateToRecent(observations), ExtractDescriptions())
                    :
                    new SyncTickers(ExtractTickers(observations), observations, ExtractDescriptions());
            }
        }

        public static SyncTickers Synchronize(IReadOnlyList<Ticker> tickers, ISimulationSettings settings)
        {
            if (tickers == null) throw new ArgumentNullException("tickers");
            if (settings == null) throw new ArgumentNullException("settings");

            string error = null;
            if (!CanSynchronize(tickers, settings, out error)) throw new ArgumentException(error);

            return new MultiTickerSynchronizer(tickers, settings).Sync();
        }

        public static bool CanSynchronize(IReadOnlyList<Ticker> tickers, ISimulationSettings settings, out string error)
        {
            if (tickers == null) throw new ArgumentNullException("tickers");
            if (settings == null) throw new ArgumentNullException("settings");

            if (tickers.Count == 0)
            {
                error = "There must be least one ticker.";
                return false;
            }

            if (tickers.Select(t => t.Name.ToLowerInvariant()).GroupBy(name => name).Count() < tickers.Count)
            {
                error = "Ticker names must be unique.";
                return false;
            }

            if (settings.Get<SimulationRangeSetting>().Type == SimulationRangeType.Common &&
                tickers.Select(t => t.From).Max() > tickers.Select(t => t.To).Min())
            {
                error = "Tickers do not have common range.";
                return false;
            }

            if (settings.Get<CommissionSetting>().Value < 0)
            {
                error = "Commission cannot be negative.";
                return false;
            }

            if (settings.Get<CommissionSetting>().Type == CommissionType.Percent &&
                settings.Get<CommissionSetting>().Value >= 100)
            {
                error = "Commission of percentage type must be smaller than 100 %.";
                return false;
            }

            if (settings.Get<SimulationRangeSetting>().Type == SimulationRangeType.FromToDates &&
                settings.Get<SimulationRangeSetting>().From > settings.Get<SimulationRangeSetting>().To)
            {
                error = "Simulation range is invalid.";
                return false;
            }

            if (settings.Get<InitialEquitySetting>().Value < 1)
            {
                error = "Initial equity cannot be smaller than 1.";
                return false;
            }

            if (settings.Get<SimulationRangeSetting>().Type == SimulationRangeType.FromToDates &&
                tickers.All(t => t.From > settings.Get<SimulationRangeSetting>().To || t.To < settings.Get<SimulationRangeSetting>().From))
            {
                error = "No quotes matching specified simulation time range.";
                return false;
            }

            error = null;
            return true;
        }
    }
}
