
namespace IstLight.Services
{
    public interface ITickerConverter : IServiceItem
    {
        string Format { get; }

        IAsyncResult<Ticker> Read(RawTicker rawTicker);
        IAsyncResult<RawTicker> Save(Ticker ticker);
    }
}
