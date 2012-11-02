
namespace IstLight.Services
{
    public interface ITickerProvider : IServiceItem
    {
        bool CanSearch { get; }
        IAsyncResult<IReadOnlyList<TickerSearchResult>> Search(string hint);
        IAsyncResult<Ticker> Get(string tickerName);
    }
}
