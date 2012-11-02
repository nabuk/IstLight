using System;

namespace IstLight
{
    public static class GeneralExtensions
    {
        public static T TypedClone<T>(this T obj) where T : ICloneable
        {
            return (T)obj.Clone();
        }
    }
}
