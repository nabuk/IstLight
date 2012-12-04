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
using System.Linq;
using System.Reflection;
using IstLight.Synchronization;

namespace IstLight.UnitTests
{
    public static class Ext
    {
        public static IReadOnlyList<T> ROL<T>(params T[] items)
        {
            return items.AsReadOnlyList();
        }

        public static Ticker ToTicker(this SimplifiedTicker simplifiedTicker, string name = null)
        {
            name = name ?? Guid.NewGuid().ToString();
            return new Ticker(name, simplifiedTicker.Select(q => new TickerQuote(q.Date, q.Value, q.Volume)).AsReadOnlyList());
        }

        public static TimeSpan Mult(this TimeSpan span, int factor) { return TimeSpan.FromTicks(span.Ticks * factor); }

        public static bool HasDefaultConstructor(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return (type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null) != null);
        }
    }
}
