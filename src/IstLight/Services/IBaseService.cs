
namespace IstLight.Services
{
    public interface IBaseService<out T>
        where T : IServiceItem
    {
        IReadOnlyList<IAsyncResult<T>> Load();
    }
}
