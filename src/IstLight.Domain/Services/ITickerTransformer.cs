
namespace IstLight.Services
{
    public interface ITickerTransformer : INamedItem
    {
        IAsyncResult<Ticker> Transform(Ticker ticker);
    }
}
