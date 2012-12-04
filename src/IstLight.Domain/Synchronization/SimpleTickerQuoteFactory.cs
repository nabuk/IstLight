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

namespace IstLight.Synchronization
{
    public static class SimpleTickerQuoteFactory
    {
        public static ISimpleTickerQuote Simplify(this ITickerQuote quote, SimulationPriceType priceType = SimulationPriceType.Close)
        {
            if (quote == null) throw new ArgumentNullException("quote");

            return new SimpleTickerQuote(quote.Date, quote.ToValue(priceType), quote.Volume);
        }

        public static double ToValue(this ITickerQuote quote, SimulationPriceType priceType = SimulationPriceType.Close)
        {
            if (quote == null) throw new ArgumentNullException("quote");

            switch (priceType)
            {
                case SimulationPriceType.Open: return quote.Open;
                case SimulationPriceType.High: return quote.High;
                case SimulationPriceType.Low: return quote.Low;
                case SimulationPriceType.Close: return quote.Close;
                case SimulationPriceType.HighLowMiddle: return (quote.High + quote.Low) / 2;
                case SimulationPriceType.OpenCloseMiddle: return (quote.Open + quote.Close) / 2;
                default: throw new NotImplementedException();
            }
        }
    }
}
