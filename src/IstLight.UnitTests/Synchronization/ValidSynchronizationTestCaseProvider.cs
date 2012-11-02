using System;
using System.Collections.Generic;
using System.Linq;
using IstLight.Settings;

namespace IstLight.UnitTests.Synchronization
{
    public class ValidSynchronizationTestCaseProvider : IEnumerable<object[]>
    {
        public class SyncArgumentBuilder
        {
            private readonly DateTime referenceDate;
            private readonly TimeSpan referenceSpan;

            private readonly List<Ticker> tickers = new List<Ticker>();
            private readonly SimulationSettings settings = new SimulationSettings();

            public SyncArgumentBuilder(DateTime? referenceDate = null, TimeSpan? referenceSpan = null, bool onlyRecentQuotes = true)
            {
                this.referenceDate = referenceDate ?? new DateTime(2000, 1, 1);
                this.referenceSpan = referenceSpan ?? TimeSpan.FromDays(1);
                settings.Get<OnlyRecentQuotesSetting>().Value = onlyRecentQuotes;
            }

            public SyncArgumentBuilder AddTicker(params int[] offsets)
            {
                tickers.Add(
                    new Ticker(
                        Guid.NewGuid().ToString(),
                        offsets.Select(o => new TickerQuote(referenceDate + referenceSpan.Mult(o), 1, 1)).AsReadOnlyList()));
                return this;
            }

            public SyncArgumentBuilder ReadNames(params TickerDescription[] descriptions)
            {
                for (int i = 0; i < descriptions.Length; i++)
                    tickers[i].Name = descriptions[i].Name;
                return this;
            }

            public SyncArgumentBuilder SetRange(SimulationRangeType rangeType, int fromOffset, int toOffset)
            {
                var rangeSetting = settings.Get<SimulationRangeSetting>();
                rangeSetting.Type = rangeType;
                rangeSetting.From = referenceDate + referenceSpan.Mult(fromOffset);
                rangeSetting.To = referenceDate + referenceSpan.Mult(toOffset);
                return this;
            }

            public SyncArgumentBuilder SetPeriod(PeriodType period)
            {
                settings.Get<PeriodSetting>().Type = period;
                return this;
            }

            public Tuple<IReadOnlyList<Ticker>, SimulationSettings> Build()
            {
                return Tuple.Create(tickers.AsReadOnlyList(), settings);
            }

            public SimulationSettings Settings { get { return this.settings; } }
        }
        
        public IEnumerator<object[]> GetEnumerator()
        {
            //IReadOnlyList<Ticker> tickers, SimulationSettings settings, SyncTickers expected, string testCase

            foreach (PeriodType period in Enum.GetValues(typeof(PeriodType)))
                foreach (var rng in new Tuple<SimulationRangeType, int, int>[]
                                    {
                                        Tuple.Create(SimulationRangeType.Common,0,1),
                                        Tuple.Create(SimulationRangeType.FromToDates,-1,0),
                                        Tuple.Create(SimulationRangeType.FromToDates,-1,1),
                                        Tuple.Create(SimulationRangeType.FromToDates,0,1)
                                    })
                    yield return SingleBarTest(period, rng.Item1, rng.Item2, rng.Item3);

            foreach (SimulationRangeType rangeType in Enum.GetValues(typeof(SimulationRangeType)))
                yield return MiddleObservationIsOmittedBecauseOfPeriod(rangeType);

            yield return PeriodImpactsObservationCreation();

            yield return NarrowedDateRangeCutsOuterQuotes();

            yield return CommonRangeCutsOuterQuotes();

            yield return TinyPeriodDoesNotIntroduceUnnecessaryData();

            yield return HugePeriodTakesOnlyBordelineData();

            yield return AboveUpperBoundTickerHasNoQuotes();

            yield return BelowLowerBoundTickerHasOneLastQuote();

            yield return NotOnlyRecentReturnsAllRelevantTickerData();

            //only when tickers can end before simulation starts
            //yield return LastObservationDateCannotBeEarlierThanRangeFromDate();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        object[] SingleBarTest(PeriodType periodType, SimulationRangeType rangeType, int rangeFromOffset = 0, int rangeToOffset=1)
        {
            var syncArgBuilder =
                new SyncArgumentBuilder()
                .AddTicker(0)
                .SetPeriod(periodType)
                .SetRange(rangeType, rangeFromOffset, rangeToOffset);

            var expectedBuilder =
                new SyncTickersBuilder()
                .AddTicker(0)
                .AddDescription()
                .AddObservation(0, 1);
            syncArgBuilder.ReadNames(expectedBuilder.Descriptions.ToArray());

            return new object[]
            {
                syncArgBuilder.Build().Item1,
                syncArgBuilder.Build().Item2,
                expectedBuilder.Build(),
                string.Format("Single bar test case. Range type = {0}, Period = {1}, From-To({2} - {3})",
                    rangeType, periodType, rangeFromOffset,rangeToOffset)
            };
        }

        object[] MiddleObservationIsOmittedBecauseOfPeriod(SimulationRangeType rangeType)
        {
            var syncArgBuilder =
                new SyncArgumentBuilder(referenceSpan: TimeSpan.FromHours(0.5))
                .AddTicker(0, 1, 2)
                .SetPeriod(PeriodType.Hour)
                .SetRange(rangeType, 0, 2);

            var expectedBuilder =
                new SyncTickersBuilder(referenceSpan: TimeSpan.FromHours(0.5))
                .AddTicker(0, 2)
                .AddDescription()
                .AddObservation(0, 1).AddObservation(2, 2);
            syncArgBuilder.ReadNames(expectedBuilder.Descriptions.ToArray());
            return new object[]
            {
                syncArgBuilder.Build().Item1,
                syncArgBuilder.Build().Item2,
                expectedBuilder.Build(),
                string.Format("Middle obseravtion is omitted because of period setting. Range type = {0}",rangeType)
            };
        }

        object[] PeriodImpactsObservationCreation()
        {
            var syncArgBuilder =
                new SyncArgumentBuilder(referenceSpan: TimeSpan.FromDays(1))
                .AddTicker(-1, 6, 13)
                .SetPeriod(PeriodType.Week)
                .SetRange(SimulationRangeType.FromToDates, 0, 14);

            var expectedBuilder =
                new SyncTickersBuilder(referenceSpan: TimeSpan.FromDays(1))
                .AddTicker(-1, 6, 13)
                .AddDescription()
                .AddObservation(0, 1).AddObservation(7, 2).AddObservation(13, 3);
            syncArgBuilder.ReadNames(expectedBuilder.Descriptions.ToArray());
            return new object[]
            {
                syncArgBuilder.Build().Item1,
                syncArgBuilder.Build().Item2,
                expectedBuilder.Build(),
                string.Format("Period impacts observation creation.")
            };
        }

        object[] NarrowedDateRangeCutsOuterQuotes()
        {
            var syncArgBuilder =
                new SyncArgumentBuilder()
                .AddTicker(-1,0,1,2)
                .SetPeriod(PeriodType.Day)
                .SetRange(SimulationRangeType.FromToDates, 0, 1);

            var expectedBuilder =
                new SyncTickersBuilder()
                .AddTicker(0, 1)
                .AddDescription()
                .AddObservation(0, 1).AddObservation(1, 2);
            syncArgBuilder.ReadNames(expectedBuilder.Descriptions.ToArray());
            return new object[]
            {
                syncArgBuilder.Build().Item1,
                syncArgBuilder.Build().Item2,
                expectedBuilder.Build(),
                string.Format("Narrowed date range cuts outer quotes.")
            };
        }

        object[] CommonRangeCutsOuterQuotes()
        {
            var syncArgBuilder =
                new SyncArgumentBuilder()
                .AddTicker(-1, 0, 1).AddTicker(0, 1, 2)
                .SetPeriod(PeriodType.Day)
                .SetRange(SimulationRangeType.Common, 0, 1);

            var expectedBuilder =
                new SyncTickersBuilder()
                .AddTicker(0, 1).AddTicker(0, 1)
                .AddDescriptions(2)
                .AddObservation(0, 1, 1).AddObservation(1, 2, 2);
            syncArgBuilder.ReadNames(expectedBuilder.Descriptions.ToArray());
            return new object[]
            {
                syncArgBuilder.Build().Item1,
                syncArgBuilder.Build().Item2,
                expectedBuilder.Build(),
                string.Format("Common range cuts outer quotes.")
            };
        }

        object[] TinyPeriodDoesNotIntroduceUnnecessaryData()
        {
            var syncArgBuilder =
                new SyncArgumentBuilder(referenceSpan: TimeSpan.FromHours(1))
                .AddTicker(0,1,2)
                .SetPeriod(PeriodType.Minute)
                .SetRange(SimulationRangeType.FromToDates, -1, 3);

            var expectedBuilder =
                new SyncTickersBuilder(referenceSpan: TimeSpan.FromHours(1))
                .AddTicker(0, 1, 2)
                .AddDescription()
                .AddObservation(0, 1).AddObservation(1, 2).AddObservation(2, 3);
            syncArgBuilder.ReadNames(expectedBuilder.Descriptions.ToArray());
            return new object[]
            {
                syncArgBuilder.Build().Item1,
                syncArgBuilder.Build().Item2,
                expectedBuilder.Build(),
                string.Format("Tiny period does not introduce unnecessary data.")
            };
        }

        object[] HugePeriodTakesOnlyBordelineData()
        {
            var syncArgBuilder =
                new SyncArgumentBuilder(referenceSpan: TimeSpan.FromHours(1))
                .AddTicker(0, 1, 2)
                .SetPeriod(PeriodType.Week)
                .SetRange(SimulationRangeType.FromToDates, -1, 3);

            var expectedBuilder =
                new SyncTickersBuilder(referenceSpan: TimeSpan.FromHours(1))
                .AddTicker(0, 2)
                .AddDescription()
                .AddObservation(0, 1).AddObservation(2, 2);
            syncArgBuilder.ReadNames(expectedBuilder.Descriptions.ToArray());
            return new object[]
            {
                syncArgBuilder.Build().Item1,
                syncArgBuilder.Build().Item2,
                expectedBuilder.Build(),
                string.Format("Huge period takes only bordeline data.")
            };
        }

        object[] AboveUpperBoundTickerHasNoQuotes()
        {
            var syncArgBuilder =
                new SyncArgumentBuilder()
                .AddTicker(0, 1).AddTicker(2, 3)
                .SetPeriod(PeriodType.Day)
                .SetRange(SimulationRangeType.FromToDates, 0, 1);

            var expectedBuilder =
                new SyncTickersBuilder()
                .AddTicker(0, 1).AddTicker()
                .AddDescriptions(2)
                .AddObservation(0, 1,0).AddObservation(1, 2, 0);
            syncArgBuilder.ReadNames(expectedBuilder.Descriptions.ToArray());
            return new object[]
            {
                syncArgBuilder.Build().Item1,
                syncArgBuilder.Build().Item2,
                expectedBuilder.Build(),
                string.Format("Above upper bound ticker has no quotes.")
            };
        }

        object[] BelowLowerBoundTickerHasOneLastQuote()
        {
            var syncArgBuilder =
                new SyncArgumentBuilder()
                .AddTicker(0, 1).AddTicker(-2, -1)
                .SetPeriod(PeriodType.Day)
                .SetRange(SimulationRangeType.FromToDates, 0, 1);

            var expectedBuilder =
                new SyncTickersBuilder()
                .AddTicker(0, 1).AddTicker(-1)
                .AddDescriptions(2)
                .AddObservation(0, 1, 1).AddObservation(1, 2, 1);
            syncArgBuilder.ReadNames(expectedBuilder.Descriptions.ToArray());
            return new object[]
            {
                syncArgBuilder.Build().Item1,
                syncArgBuilder.Build().Item2,
                expectedBuilder.Build(),
                string.Format("Below lower bound ticker has one quote.")
            };
        }

        object[] NotOnlyRecentReturnsAllRelevantTickerData()
        {
            var syncArgBuilder =
                new SyncArgumentBuilder(onlyRecentQuotes: false)
                .AddTicker(-1, 0, 1, 2, 3, 4, 5)
                .SetPeriod(PeriodType.Week)
                .SetRange(SimulationRangeType.FromToDates, 0, 4);

            var expectedBuilder =
                new SyncTickersBuilder()
                .AddTicker(-1, 0, 1, 2, 3, 4)
                .AddDescriptions(1)
                .AddObservation(0, 2).AddObservation(4, 6);
            syncArgBuilder.ReadNames(expectedBuilder.Descriptions.ToArray());

            return new object[]
            {
                syncArgBuilder.Build().Item1,
                syncArgBuilder.Build().Item2,
                expectedBuilder.Build(),
                string.Format("Not only recent returns all relevant ticker data.")
            };

        }

        object[] LastObservationDateCannotBeEarlierThanRangeFromDate()
        {
            var syncArgBuilder =
                new SyncArgumentBuilder()
                .AddTicker(-1)
                .SetPeriod(PeriodType.Day)
                .SetRange(SimulationRangeType.FromToDates, 0, 1);

            var expectedBuilder =
                new SyncTickersBuilder()
                .AddTicker(-1)
                .AddDescriptions(1)
                .AddObservation(0, 1);
            syncArgBuilder.ReadNames(expectedBuilder.Descriptions.ToArray());
            return new object[]
            {
                syncArgBuilder.Build().Item1,
                syncArgBuilder.Build().Item2,
                expectedBuilder.Build(),
                string.Format("Last observation date cannot be earlier than range from date.")
            };
        }
    }
}
