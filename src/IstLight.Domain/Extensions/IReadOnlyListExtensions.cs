// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;

namespace IstLight
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
