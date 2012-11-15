using System;
using System.Collections.Concurrent;

namespace IstLight.Services
{
    public class OneTimeCallbackContainer<T>
    {
        private readonly ConcurrentBag<Action<T>> callbacks = new ConcurrentBag<Action<T>>();

        public void AddCallback(Action<T> callback)
        {
            callbacks.Add(callback);
        }

        public void FireCallbacks(T arg)
        {
            Action<T> callback;

            while (callbacks.TryTake(out callback))
                FireCallback(callback, arg);
        }

        protected virtual void FireCallback(Action<T> callback, T arg)
        {
            callback(arg);
        }
    }
}
