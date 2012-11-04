using System;
using IstLight.Settings;
using IstLight.Synchronization;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests.Synchronization
{
    public class SimpleTickerQuoteFactoryTests
    {
        [Fact]
        public void Simplify_AsITickerQuoteExtension_NullQuotePassed_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => SimpleTickerQuoteFactory.Simplify(null, SimulationPriceType.Close));
        }

        [Fact]
        public void ToValue_NullQuotePassed_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => SimpleTickerQuoteFactory.ToValue(null, SimulationPriceType.Close));
        }

        [Theory]
        [ClassData(typeof(TickerQuoteWithPriceProvider))]
        public void Simplify_AsITickerQuoteExtension_AssignsVolume(ITickerQuote tickerQuote, SimulationPriceType priceType)
        {
            var sut = SimpleTickerQuoteFactory.Simplify(tickerQuote, priceType);

            Assert.Equal<double?>(tickerQuote.Volume, sut.Volume);
        }

        [Theory]
        [ClassData(typeof(TickerQuoteWithPriceProvider))]
        public void Simplify_AsITickerQuoteExtension_AssignsCorrectValue(ITickerQuote tickerQuote, SimulationPriceType priceType)
        {
            var sut = SimpleTickerQuoteFactory.Simplify(tickerQuote, priceType);

            Assert.Equal<double?>(ExpectedFromPrice(tickerQuote,priceType), sut.Value);
        }

        [Theory]
        [ClassData(typeof(TickerQuoteWithPriceProvider))]
        public void Simplify_AsITickerQuoteExtension_AssignsDate(ITickerQuote tickerQuote, SimulationPriceType priceType)
        {
            var sut = SimpleTickerQuoteFactory.Simplify(tickerQuote, priceType);

            Assert.Equal<DateTime>(tickerQuote.Date, sut.Date);
        }

        [Theory]
        [ClassData(typeof(TickerQuoteWithPriceProvider))]
        public void ToValue_ComputesCorrectValue(ITickerQuote tickerQuote, SimulationPriceType priceType)
        {
            Assert.Equal<double?>(ExpectedFromPrice(tickerQuote, priceType), SimpleTickerQuoteFactory.ToValue(tickerQuote, priceType));
        }

        public double ExpectedFromPrice(ITickerQuote quote, SimulationPriceType priceType)
        {
            switch (priceType)
            {
                case SimulationPriceType.Open: return quote.Open;
                case SimulationPriceType.Close: return quote.Close;
                case SimulationPriceType.High: return quote.High;
                case SimulationPriceType.Low: return quote.Low;
                case SimulationPriceType.HighLowMiddle: return (quote.High + quote.Low) / 2;
                case SimulationPriceType.OpenCloseMiddle: return (quote.Open + quote.Close) / 2;
                default: throw new NotImplementedException();
            }
        }
    }
}
