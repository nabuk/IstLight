
namespace IstLight.Services
{
    public interface ITickerService
    {
        IReadOnlyList<IAsyncResult<Ticker>> Load();
        IAsyncResult<bool> Save(Ticker ticker);
    }
}
