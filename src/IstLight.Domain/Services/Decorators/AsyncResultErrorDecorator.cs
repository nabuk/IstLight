using System;

namespace IstLight.Services.Decorators
{
    public class AsyncResultErrorDecorator<T> : IAsyncResult<T>
    {
        private readonly IAsyncResult<T> arToDecorate;
        private readonly Func<T, T> decorate;
        private readonly IErrorReporter reporter;

        private T decoratedResult;
        private bool isResultDecorated;

        public AsyncResultErrorDecorator(IAsyncResult<T> arToDecorate, IErrorReporter reporter, Func<T, T> decorateResult = null)
        {
            this.arToDecorate = arToDecorate;
            this.decorate = decorateResult ?? (x => x);
            this.reporter = reporter;

            arToDecorate.AddCallback(x => reporter.AddIfNotNull(x.Error));
        }

        public T Result
        {
            get
            {
                if (!arToDecorate.IsCompleted || arToDecorate.Error != null)
                    return default(T);

                if (!isResultDecorated)
                {
                    decoratedResult = decorate(arToDecorate.Result);
                    isResultDecorated = true;
                }

                return decoratedResult;
            }
        }

        public Exception Error
        {
            get { return arToDecorate.Error; }
        }

        public bool IsCompleted
        {
            get { return arToDecorate.IsCompleted; }
        }

        public bool Wait(int timeout)
        {
            return arToDecorate.Wait(timeout);
        }

        public void AddCallback(Action<IAsyncResult<T>> callback)
        {
            arToDecorate.AddCallback(x => callback(this));
        }
    }
}
