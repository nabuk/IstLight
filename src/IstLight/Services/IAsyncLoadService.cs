
namespace IstLight.Services
{
    public interface IAsyncLoadService<out T>
        where T : INamedItem
    {
        IReadOnlyList<IAsyncResult<T>> Load();
    }
}
