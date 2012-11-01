using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Services
{
    public interface IBaseRepository<out T>
        where T : IRepositoryItem
    {
        IReadOnlyList<IAsyncResult<T>> Load();
    }
}
