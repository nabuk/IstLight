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
using IstLight.Settings;
using IstLight.Simulation;
using IstLight.Synchronization;
using IstLight.UnitTests.Synchronization;
using Xunit;

namespace IstLight.UnitTests.Simulation
{
    public class SimulationResultTests
    {
        [Fact]
        public void ctor_Quotes_ArePassedToBaseConstructor()
        {
            var args = CreateSut();
            var sut = args.Item1;
            var quotes = args.Item2;

            Assert.True(quotes.SequenceEqual(sut));
        }

        [Fact]
        public void ctor_Descriptions_ArePassedToBaseConstructor()
        {
            var args = CreateSut();
            var sut = args.Item1;
            var descriptions = args.Item1.Descriptions;

            Assert.True(descriptions.SequenceEqual(sut.Descriptions));
        }

        [Fact]
        public void ctor_SyncTickers_AreSet()
        {
            var args = CreateSut();
            var sut = args.Item1;
            var syncTickers = args.Item3;

            Assert.Equal<SyncTickers>(syncTickers, sut.SyncTickers);
        }

        [Fact]
        public void Settings_CanSet()
        {
            var sut = CreateSut().Item1;
            var settigs = new SimulationSettings();
            sut.Settings = settigs;
            
            Assert.Same(settigs, sut.Settings);
        }

        public static Tuple<SimulationResult,
            IReadOnlyList<SimulationResultQuote>,
            SyncTickers> CreateSut()
        {
            var quotes = new SimulationResultQuote[] { SimulationResultQuoteTests.CreateSut() }.AsReadOnlyList();
            var syncTickers = new SyncTickersBuilder().AddTicker(0).AddObservation(0, 1).AddDescription().Build();
            
            return Tuple.Create(new SimulationResult(quotes, syncTickers), quotes, syncTickers);
        }
    }
}
