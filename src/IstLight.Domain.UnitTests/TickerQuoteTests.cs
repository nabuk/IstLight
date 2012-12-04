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
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests
{
    public class TickerQuoteTests
    {

        [Fact]
        public void ctor_ZeroVolume_IsConvertedToNull()
        {
            var sut = CreateByLongCtor(volume: 0);
            Assert.Null(sut.Volume);
        }

        [Fact]
        public void ctor_NegativeVolume_Throws()
        {
            Assert.Throws<ArgumentException>(() => CreateByLongCtor(volume:-1));
        }

        [Fact]
        public void shortCtor_ValueArgument_SetsOpen()
        {
            var value = 1;
            Assert.Equal<double>(value, CreateByShortCtor(value: value).Open);
        }

        [Fact]
        public void shortCtor_ValueArgument_SetsClose()
        {
            var value = 2;
            Assert.Equal<double>(value, CreateByShortCtor(value: value).Close);
        }

        [Fact]
        public void shortCtor_ValueArgument_SetsHigh()
        {
            var value = 3;
            Assert.Equal<double>(value, CreateByShortCtor(value: value).High);
        }

        [Fact]
        public void shortCtor_ValueArgument_SetsLow()
        {
            var value = 4;
            Assert.Equal<double>(value, CreateByShortCtor(value: value).Low);
        }

        [Fact]
        public void ctor_DateArgument_IsSet()
        {
            DateTime date = DateTime.Now;
            Assert.Equal<DateTime>(date, CreateByShortCtor(date: date).Date);            
        }

        [Fact]
        public void ctor_VolumeArgument_IsSet()
        {
            double? volume = 2;
            Assert.Equal<double?>(volume, CreateByShortCtor(volume: volume).Volume);
        }

        [Fact]
        public void ctor_NullVolumeArgument_IsSetToNull()
        {
            double? volume = null;
            Assert.Equal<double?>(volume, CreateByShortCtor(volume: volume).Volume);
        }

        [Fact]
        public void Implements_ITickerQuote()
        {
            Assert.IsAssignableFrom<ITickerQuote>(CreateByShortCtor());
        }

        [Theory]
        [InlineData(1, 5, 4, 1)]
        [InlineData(1, 5, 0, 1)]
        [InlineData(1, 5, 5, 2)]
        [InlineData(1, 5, 5, 6)]
        [InlineData(5, 1, 4, 1)]
        [InlineData(5, 1, 0, 1)]
        [InlineData(5, 1, 5, 2)]
        [InlineData(5, 1, 5, 6)]
        public void ctor_InvalidHighLowArguments_Throws(double open, double close, double high, double low)
        {
            Assert.Throws<ArgumentException>(() => CreateByLongCtor(open: open, close: close, high: high, low: low));
        }

        [Theory]
        [InlineData(6, 5, 8, 1)]
        [InlineData(5, 6, 6, 5)]
        [InlineData(0, 0, 0, 0)]
        [InlineData(-2, -3, -2, -3)]
        [InlineData(-2, 2, 4, -8)]
        public void ctor_ValidPriceArguments_DoesNotThrow(double open, double close, double high, double low)
        {
            Assert.DoesNotThrow(() => CreateByLongCtor(open: open, close: close, high: high, low: low));
        }

        #region Factories
        public static TickerQuote CreateByShortCtor(
            DateTime? date = null,
            double value = 1,
            double? volume = 1)
        {
            return new TickerQuote(date ?? DateTime.Now, value, volume);
        }

        public static TickerQuote CreateByLongCtor(
            DateTime? date = null,
            double open = 1,
            double high = 1,
            double low = 1,
            double close = 1,
            double? volume = 1)
        {
            return new TickerQuote(date ?? DateTime.Now, open, close,high,low, volume);
        }
        #endregion
    }
}
