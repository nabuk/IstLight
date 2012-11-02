using System;

namespace IstLight.Services
{
    public interface IAsyncResult<out T>
    {
        T Result { get; }
        Exception Error { get; }
        bool IsCompleted { get; }
        bool Wait(int timeout);
        void AddCallback(Action<IAsyncResult<T>> callback);
    }
}
