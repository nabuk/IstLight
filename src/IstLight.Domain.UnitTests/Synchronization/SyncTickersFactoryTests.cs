using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Extensions;
using IstLight.Domain.Synchronization;
using IstLight.Domain.Settings;
using Xunit;
using Xunit.Extensions;

namespace IstLight.Domain.UnitTests.Synchronization
{
    public class SyncTickersFactoryTests
    {
        [Fact]
        public void Synchronize_NullTickers_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => SyncTickersFactory.Synchronize(
                    null,
                    new SimulationSettings()));
        }

        [Fact]
        public void Synchronize_NullSettings_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => SyncTickersFactory.Synchronize(
                    new Ticker[] { TickerTests.CreateTicker() }.AsReadOnlyList(),
                    null));
        }

        [Fact]
        public void CanSynchronize_NullTickers_Throws()
        {
            string err;
            Assert.Throws<ArgumentNullException>(
                () => SyncTickersFactory.CanSynchronize(
                    null,
                    new SimulationSettings(),
                    out err));
        }

        [Fact]
        public void CanSynchronize_NullSettings_Throws()
        {
            string err;
            Assert.Throws<ArgumentNullException>(
                () => SyncTickersFactory.CanSynchronize(
                    new Ticker[] { TickerTests.CreateTicker() }.AsReadOnlyList(),
                    null,
                    out err));
        }

        [Theory]
        [ClassData(typeof(InvalidSynchronizationTestCaseProvider))]
        public void CanSynchronize_WrongTestCase_ReturnsFalse(IReadOnlyList<Ticker> tickers, SimulationSettings settings, string assertFailMessage)
        {
            string error;
            Assert.False(SyncTickersFactory.CanSynchronize(tickers, settings, out error));
        }

        [Theory]
        [ClassData(typeof(InvalidSynchronizationTestCaseProvider))]
        public void CanSynchronize_WrongTestCase_SetsNotEmptyError(IReadOnlyList<Ticker> tickers, SimulationSettings settings, string assertFailMessage)
        {
            string error;
            SyncTickersFactory.CanSynchronize(tickers, settings, out error);
            Assert.False(string.IsNullOrEmpty(error), "Error string is empty but should contain explanation message.");
        }

        [Theory]
        [ClassData(typeof(InvalidSynchronizationTestCaseProvider))]
        public void Synchronize_SynchronizationNotPossible_Throws(IReadOnlyList<Ticker> tickers, SimulationSettings settings, string assertFailMessage)
        {
            Exception otherException = null;
            bool thrownArgumentException = false;

            try { SyncTickersFactory.Synchronize(tickers, settings); }
            catch (ArgumentException) { thrownArgumentException = true; }
            catch (Exception ex) { otherException = ex; }
            finally
            {
                Assert.True(thrownArgumentException,
                    "Expected ArgumentException after calling SyncTickersFactory.Synchronize. Test case: " + assertFailMessage +
                    (otherException == null ? "" : ". Got different exception: " + otherException.Message.ToString())
                    );
            }
        }

        [Theory(Timeout = 200)]
        [ClassData(typeof(ValidCanSynchronizeTestCaseProvider))]
        public void CanSynchronize_SynchronizationPossible_ReturnsTrue(IReadOnlyList<Ticker> tickers, SimulationSettings settings, string testCase)
        {
            string error;
            Assert.True(SyncTickersFactory.CanSynchronize(tickers, settings, out error));
        }

        [Theory(Timeout = 200)]
        [ClassData(typeof(ValidSynchronizationTestCaseProvider))]
        public void Synchronize_ObservationsAreCorrect(IReadOnlyList<Ticker> tickers, SimulationSettings settings, SyncTickers expected, string testCase)
        {
            Func<IEnumerable<Observation>, string> printer = x => x.Select((o, iO) => string.Format("[{0}: ({2}) {1}]", iO, o.Date,
                    o.CurrentQuoteCount.Select(i => i.ToString()).Aggregate((s1, s2) => s1 + "," + s2))).Aggregate((s1, s2) => s1 + "," + s2);

            var sut = SyncTickersFactory.Synchronize(tickers, settings);
            Assert.True(expected.SequenceEqual(sut, new ObservationTests.ObservationComparer()),
                "Observation are not correct."+
                " Expected: "+printer(expected)+
                ". Got: "+printer(sut));
        }

        [Theory(Timeout = 200)]
        [ClassData(typeof(ValidSynchronizationTestCaseProvider))]
        public void Synchronize_DescriptionsAreCorrect(IReadOnlyList<Ticker> tickers, SimulationSettings settings, SyncTickers expected, string testCase)
        {
            var sut = SyncTickersFactory.Synchronize(tickers, settings);
            Assert.True(expected.Descriptions.SequenceEqual(sut.Descriptions, new TickerDescriptionTests.TickerDescriptionComparer()),
                "Ticker descriptions are not correct.");
        }

        [Theory(Timeout = 200)]
        [ClassData(typeof(ValidSynchronizationTestCaseProvider))]
        public void Synchronize_TickersAreCorrect(IReadOnlyList<Ticker> tickers, SimulationSettings settings, SyncTickers expected, string testCase)
        {
            var sut = SyncTickersFactory.Synchronize(tickers, settings);
            Assert.True(expected.SimplifiedTickers.SequenceEqual(sut.SimplifiedTickers, new SimplifiedTickerTests.SimplifiedTickerComparer()),
                "Simplified tickers are not correct.");
        }
    }
}
