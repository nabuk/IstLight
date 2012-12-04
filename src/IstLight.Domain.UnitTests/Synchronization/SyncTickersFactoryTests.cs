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
using IstLight.Settings;
using IstLight.Synchronization;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests.Synchronization
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
