using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Extensions
{
    public static class GeneralExtensions
    {
        public static T TypedClone<T>(this T obj) where T : ICloneable
        {
            return (T)obj.Clone();
        }
    }
}
