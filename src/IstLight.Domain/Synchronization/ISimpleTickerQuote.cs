
namespace IstLight.Synchronization
{
    public interface ISimpleTickerQuote : IDate
    {
        double Value { get; }
        double? Volume { get; }
    }
}
