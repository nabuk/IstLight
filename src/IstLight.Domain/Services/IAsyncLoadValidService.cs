using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Services
{
    public interface IAsyncLoadValidService<out T>
        where T : INamedItem
    {
        void AddCallback(Action<T> callback);
        void Load();
    }
}
