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
