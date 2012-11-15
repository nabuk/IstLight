using System.Linq;

namespace IstLight.Services.Decorators
{
    public class AsyncLoadServiceDecorator<T> : IAsyncLoadService<T>
        where T : INamedItem
    {
        private readonly IAsyncLoadService<T> asyncLoadService;
        private readonly IAsyncResultDecoratorFactory<T> decoratorFactory;

        public AsyncLoadServiceDecorator(IAsyncLoadService<T> asyncLoadService, IAsyncResultDecoratorFactory<T> decoratorFactory)
        {
            this.asyncLoadService = asyncLoadService;
            this.decoratorFactory = decoratorFactory;
        }

        public IReadOnlyList<IAsyncResult<T>> Load()
        {
            return asyncLoadService.Load().Select(ar => decoratorFactory.Decorate(ar)).AsReadOnlyList();
        }
    }
}
