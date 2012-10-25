using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Extensions
{
    public static class IReadOnlyListExtensions
    {
        public static IReadOnlyList<T> AsReadOnlyList<T>(this IList<T> list)
        {
            return new ProxiedReadOnlyList<T>(i => list[i], () => list.Count);
        }

        public static IReadOnlyList<T> AsReadOnlyList<T>(this T[] array)
        {
            return new ProxiedReadOnlyList<T>(i => array[i], () => array.Length);
        }

        public static IReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ToArray().AsReadOnlyList();
        }

        public static IReadOnlyList<T> Reverse<T>(this IReadOnlyList<T> readOnlyList)
        {
            return new ProxiedReadOnlyList<T>(i => readOnlyList[readOnlyList.Count - 1 - i], () => readOnlyList.Count);
        }

        public static IReadOnlyList<T> Take<T>(this IReadOnlyList<T> readOnlyList, int count)
        {
            count = Math.Min(count, readOnlyList.Count);
            return new ProxiedReadOnlyList<T>(i => readOnlyList[i], () => count);

        }
    }
}
