using System;
using System.Collections.Generic;
using System.Linq;

namespace IstLight.Extensions
{
    public static class DictionaryExtensions
    {
        public static TKey ValueToKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue value)
        {
            return dict.First(kvp => Object.Equals(kvp.Value, value)).Key;
        }
    }
}
