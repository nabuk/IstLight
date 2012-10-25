using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain
{
    public interface IReadOnlyList<out T> : IEnumerable<T>
    {
        T this[int index] { get; }
        int Count { get; }
    }
}
