using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Strategy;

namespace IstLight.Extensions
{
    public static class IQuoteContextExtensions
    {
        public static double? GetCurrentPrice(this IQuoteContext ctx, int tickerIndex)
        {
            var quotes = ctx.GetQuotes(tickerIndex);
            return quotes.Count > 0 ? (double?)quotes[0].Value : null;
        }
    }
}
