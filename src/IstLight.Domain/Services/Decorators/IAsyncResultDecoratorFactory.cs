using System;

namespace IstLight.Services.Decorators
{
    public interface IAsyncResultDecoratorFactory<T>
    {
        IAsyncResult<T> Decorate(IAsyncResult<T> arToDecorate);
    }

    public class DelegateAsyncResultDecoratorFactory<T> : IAsyncResultDecoratorFactory<T>
    {
        private readonly Func<IAsyncResult<T>, IAsyncResult<T>> decorate;

        public DelegateAsyncResultDecoratorFactory(Func<IAsyncResult<T>, IAsyncResult<T>> decorate = null)
        {
            this.decorate = decorate ?? (x => x);
        }

        public IAsyncResult<T> Decorate(IAsyncResult<T> arToDecorate)
        {
            return decorate(arToDecorate);
        }
    }
}
