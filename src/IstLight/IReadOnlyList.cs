using System.Collections.Generic;

namespace IstLight
{
    public interface IReadOnlyList<out T> : IEnumerable<T>
    {
        T this[int index] { get; }
        int Count { get; }
    }
}
