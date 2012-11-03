
namespace IstLight.Services
{
    public interface ITickerProvider : INamedItem
    {
        bool CanSearch { get; }
        IAsyncResult<IReadOnlyList<TickerSearchResult>> Search(string hint);
        IAsyncResult<Ticker> Get(string tickerName);
    }
}
