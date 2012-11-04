using System.Collections.Generic;

namespace IstLight.UnitTests
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
