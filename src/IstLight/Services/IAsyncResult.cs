using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
