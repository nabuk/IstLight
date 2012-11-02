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
