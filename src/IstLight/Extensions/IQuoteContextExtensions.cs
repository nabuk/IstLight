
namespace IstLight.Strategy
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
