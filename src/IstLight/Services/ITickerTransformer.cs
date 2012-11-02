
namespace IstLight.Services
{
    public interface ITickerTransformer : IServiceItem
    {
        IAsyncResult<Ticker> Transform(Ticker ticker);
    }
}
