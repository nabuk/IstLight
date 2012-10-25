using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.UnitTests
{
    public static class IReadOnlyListTestsExtensions
    {
        public static IEnumerable<T> GetIEnumerableThroughIndexing<T>(this IReadOnlyList<T> collection)
        {
            for (int i = 0; i < collection.Count; i++)
                yield return collection[i];
        }
    }
}
