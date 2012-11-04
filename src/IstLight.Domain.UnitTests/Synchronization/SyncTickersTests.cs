using System;
using System.Collections.Generic;
using System.Linq;
using IstLight.Synchronization;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests.Synchronization
{
    public class SyncTickersTests
    {
        [Fact]
        public void ctor_NullSimplifiedTickers_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new SyncTickers(
                null,
                Ext.ROL(ObservationTests.CreateObservation(DateTime.Now, 0)),
                Ext.ROL(new TickerDescription { Name = "X" })));
        }

        [Fact]
        public void ctor_SimplifiedTickers_AreSet()
        {
            var tickers = Ext.ROL(SimplifiedTickerTests.CreateTicker(0));
            var sut = new SyncTickers(
                tickers,
                Ext.ROL(ObservationTests.CreateObservation(DateTime.Now, 0)),
                Ext.ROL(new TickerDescription { Name = "X" }));
            
            Assert.Same(tickers, sut.SimplifiedTickers);
        }

        public static IEnumerable<object[]> NotMatchingTickerCount_TestCases
        {
            get
            {
                yield return new object[]
                {
                    "2 tickers, 1 observation, 1 description",
                    (Action)(() => new SyncTickersBuilder()
                    .AddTicker(0).AddTicker(0)
                    .AddObservation(0,1)
                    .AddDescriptions(1).Build())
                };

                yield return new object[]
                {
                    "1 ticker, 1 observation about 2 tickers, 1 description",
                    (Action)(() => new SyncTickersBuilder()
                    .AddTicker(0)
                    .AddObservation(0, 1, 1)
                    .AddDescriptions(1).Build())
                };

                yield return new object[]
                {
                    "1 ticker, 1 observation, 2 descriptions",
                    (Action)(() => new SyncTickersBuilder()
                    .AddTicker(0)
                    .AddObservation(0, 1)
                    .AddDescriptions(2).Build())
                };

                yield return new object[]
                {
                    "2 tickes, 2 observations (2 tickers and 1 ticker), 2 descriptions",
                    (Action)(() => new SyncTickersBuilder()
                    .AddTicker(0).AddTicker(0)
                    .AddObservation(0, 1, 1).AddObservation(1, 1)
                    .AddDescriptions(2).Build())
                };
            }
        }

        [Theory, PropertyData("NotMatchingTickerCount_TestCases")]
        public void ctor_NotMatchingTickerCount_Throws(string testCastDescription, Action buildAction)
        {
            Assert.Throws<ArgumentException>(() => buildAction());
        }

        public static IEnumerable<object[]> ObservedQuotes_WrongIndex_TestCases
        {
            get
            {
                var syncTickers =
                    new SyncTickersBuilder()
                    .AddTicker(0)
                    .AddObservation(0, 1)
                    .AddDescriptions(1)
                    .Build();

                yield return new object[] { syncTickers, 2 };
                yield return new object[] { syncTickers, -1 };
            }
        }

        [Theory, PropertyData("ObservedQuotes_WrongIndex_TestCases")]
        public void LastObservedQuote_WrongTickerIndex_Throws(SyncTickers sut, int tickerIndex)
        {
            Assert.Throws<ArgumentException>(() => sut.LastObservedQuote(tickerIndex,0));
        }

        [Theory, PropertyData("ObservedQuotes_WrongIndex_TestCases")]
        public void LastObservedQuote_WrongObservationIndex_Throws(SyncTickers sut,int observationIndex)
        {
            Assert.Throws<ArgumentException>(() => sut.LastObservedQuote(0, observationIndex));
        }

        [Theory, PropertyData("ObservedQuotes_WrongIndex_TestCases")]
        public void ObservedQuotes_WrongTickerIndex_Throws(SyncTickers sut, int tickerIndex)
        {
            Assert.Throws<ArgumentException>(() => sut.ObservedQuotes(tickerIndex, 0));
        }

        [Theory, PropertyData("ObservedQuotes_WrongIndex_TestCases")]
        public void ObservedQuotes_WrongObservationIndex_Throws(SyncTickers sut, int observationIndex)
        {
            Assert.Throws<ArgumentException>(() => sut.ObservedQuotes(0, observationIndex));
        }

        public static IEnumerable<object[]> ObservedQuotes_ReturnsCorrectData_TestCases
        {
            get
            {
                var builder = new SyncTickersBuilder()
                .AddTicker(0, 2, 3)
                .AddTicker(1, 2, 4)
                .AddTicker()
                .AddObservation(0, 1, 0, 0)
                .AddObservation(1, 1, 1, 0)
                .AddObservation(2, 2, 2, 0)
                .AddObservation(3, 3, 2, 0)
                .AddObservation(4, 3, 3, 0);
                builder.AddDescriptions(builder.Tickers.Count);

                for (int tickerIndex = 0; tickerIndex < builder.Tickers.Count; tickerIndex++)
                    for (int observationIndex = 0; observationIndex < builder.Observations.Count; observationIndex++)
                    {
                        var expectedCollection =
                            builder
                            .Tickers[tickerIndex]
                            .Take(builder.Observations[observationIndex].CurrentQuoteCount[tickerIndex])
                            .AsReadOnlyList();

                        yield return new object[]
                        {
                            builder.Build(),
                            tickerIndex,
                            observationIndex,
                            expectedCollection
                        };
                    }
            }
        }

        [Theory, PropertyData("ObservedQuotes_ReturnsCorrectData_TestCases")]
        public void ObservedQuotes_ReturnsCorrectData(
            SyncTickers sut,
            int tickerIndex,
            int observationIndex,
            IReadOnlyList<ISimpleTickerQuote> expected)
        {
            Assert.True(expected.SequenceEqual(sut.ObservedQuotes(tickerIndex, observationIndex)));
        }

        [Theory, PropertyData("ObservedQuotes_ReturnsCorrectData_TestCases")]
        public void LastObservedQuote_ReturnsCorrectData(
            SyncTickers sut,
            int tickerIndex,
            int observationIndex,
            IReadOnlyList<ISimpleTickerQuote> expected)
        {
            Assert.Equal<ISimpleTickerQuote>(expected.LastOrDefault(), sut.LastObservedQuote(tickerIndex, observationIndex));
        }
    }
}
