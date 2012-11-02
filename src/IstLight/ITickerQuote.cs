
namespace IstLight
{
    public interface ITickerQuote : IDate
    {
        double Open { get; }
        double Close { get; }
        double High { get; }
        double Low { get; }

        double? Volume { get; }
    }
}
