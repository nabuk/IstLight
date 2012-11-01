using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Services
{
    public interface IBaseService<out T>
        where T : IServiceItem
    {
        IReadOnlyList<IAsyncResult<T>> Load();
    }
}
