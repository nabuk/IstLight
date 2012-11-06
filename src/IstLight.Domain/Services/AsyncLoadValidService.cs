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
        private readonly Action<Exception> errorCallback;
        private readonly IAsyncLoadService<T> asyncLoadService;

        public AsyncLoadValidService(IAsyncLoadService<T> asyncLoadService, Action<Exception> errorCallback)
        {
            this.errorCallback = errorCallback;
            this.asyncLoadService = asyncLoadService;
        }

        #region IAsyncLoadValidService<T>
        public void AttachCallback(Action<T> callback)
        {
            callbacks.Add(callback);
        }

        public void Load()
        {
            foreach (var ar in asyncLoadService.Load())
                ar.AddCallback(HandleLoaded);
        }
        #endregion

        private void HandleLoaded(IAsyncResult<T> item)
        {
            if (item.Error != null && errorCallback != null)
                errorCallback(item.Error);
            else foreach (Action<T> callback in callbacks)
                    callback(item.Result);
        }
    }
}
