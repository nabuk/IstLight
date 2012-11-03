
namespace IstLight.Services
{
    public interface ITickerConverter : INamedItem
    {
        string Format { get; }

        IAsyncResult<Ticker> Read(RawTicker rawTicker);
        IAsyncResult<RawTicker> Save(Ticker ticker);
    }
}
