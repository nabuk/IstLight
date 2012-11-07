using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Services
{
    public class AsyncLoadValidService<T> : IAsyncLoadValidService<T>
        where T : INamedItem
    {
        private readonly ConcurrentBag<Action<T>> callbacks = new ConcurrentBag<Action<T>>();
        private readonly IErrorReporter errorReporter;
        private readonly IAsyncLoadService<T> asyncLoadService;

        private void HandleLoaded(IAsyncResult<T> item)
        {
            if (item.Error != null && errorReporter != null)
                errorReporter.Add(item.Error);
            else foreach (Action<T> callback in callbacks)
                    callback(item.Result);
        }

        public AsyncLoadValidService(IAsyncLoadService<T> asyncLoadService, IErrorReporter errorReporter = null)
        {
            if (asyncLoadService == null) throw new ArgumentNullException("asyncLoadService");

            this.errorReporter = errorReporter;
            this.asyncLoadService = asyncLoadService;
        }

        #region IAsyncLoadValidService<T>
        public void AddCallback(Action<T> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");
            callbacks.Add(callback);
        }

        public void Load()
        {
            foreach (var ar in asyncLoadService.Load())
                ar.AddCallback(HandleLoaded);
        }
        #endregion

        
    }
}
