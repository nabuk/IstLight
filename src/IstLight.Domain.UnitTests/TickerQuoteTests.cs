using System;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests
{
    public class TickerQuoteTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ctor_NegativeVolume_Throws(double volume)
        {
            Assert.Throws<ArgumentException>(() => CreateByLongCtor(volume: volume));
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
