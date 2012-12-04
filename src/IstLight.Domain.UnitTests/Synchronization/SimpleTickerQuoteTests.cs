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
using IstLight.Synchronization;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests.Synchronization
{
    public class SimpleTickerQuoteTests
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        public void ctor_NonPositiveVolume_Throws(double value, double volume)
        {
            Assert.Throws<ArgumentException>(() => new SimpleTickerQuote(DateTime.Now, value, volume));
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(0.0)]
        [InlineData(-1.0)]
        public void ctor_Value_IsSet(double value)
        {
            var sut = new SimpleTickerQuote(DateTime.Now, value, null);
            Assert.Equal<double?>(value, sut.Value);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(null)]
        public void ctor_Volume_IsSet(double? volume)
        {
            var sut = new SimpleTickerQuote(DateTime.Now, 1, volume);
            Assert.Equal<double?>(volume, sut.Volume);
        }

        [Fact]
        public void ctor_Date_IsSet()
        {
            var date = new DateTime(2000,1,1);
            var sut = new SimpleTickerQuote(date, 1, null);
            Assert.Equal<DateTime>(date, sut.Date);
        }
    }
}
