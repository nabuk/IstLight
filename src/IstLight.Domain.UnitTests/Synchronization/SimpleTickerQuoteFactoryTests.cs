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
