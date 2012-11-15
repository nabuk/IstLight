using System;
using System.Threading.Tasks;

namespace IstLight.Services
{
    public class AsyncOneTimeCallbackContainer<T> : OneTimeCallbackContainer<T>
    {
        protected override void FireCallback(Action<T> callback, T arg)
        {
            Task.Factory.StartNew(x => (x as Action<T>)(arg), callback);
        }
    }
}
