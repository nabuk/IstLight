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
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests
{
    public class TickerDescriptionTests
    {
        [Theory, AutoMockingData]
        public void NameProperty_GetSetWorks(string value, TickerDescription sut)
        {
            sut.Name = value;
            Assert.Equal<string>(value, sut.Name);
        }

        [Theory]
        [InlineAutoData(true)]
        [InlineAutoData(false)]
        public void CanBeBoughtProperty_GetSetWorks(bool value, TickerDescription sut)
        {
            sut.CanBeBought = value;
            Assert.Equal<bool>(value, sut.CanBeBought);
        }

        [Fact]
        public void GetDescription_HasCorrectName()
        {
            string tickerName = new Fixture().CreateAnonymous<string>();
            var ticker = TickerTests.CreateTicker(name: tickerName);
            var sut = ticker.GetDescription();

            Assert.Equal<string>(tickerName, sut.Name);
        }

        [Fact]
        public void GetDescription_ChangingTickerName_AffectsDescription()
        {
            var ticker = TickerTests.CreateTicker();
            string newTickerName = new Fixture().CreateAnonymous<string>();
            ticker.Name = newTickerName;
            var sut = ticker.GetDescription();

            Assert.Equal<string>(newTickerName, sut.Name);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetDescription_HasCorrectCanBeBought(bool expected)
        {
            var ticker = TickerTests.CreateTickerProvidingQuotes(quotes: expected ? new double[] { 2, 1 } : new double[] { 1, 0 });
            var sut = ticker.GetDescription();

            Assert.Equal<bool>(expected, sut.CanBeBought);
        }

        public class TickerDescriptionComparer : IEqualityComparer<TickerDescription>
        {
            public bool Equals(TickerDescription x, TickerDescription y)
            {
                return string.Equals(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(TickerDescription obj)
            {
                return obj.Name.GetHashCode();
            }
        }
    }
}
