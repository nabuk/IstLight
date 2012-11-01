using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using IstLight.Extensions;
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
