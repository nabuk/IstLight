using System;
using System.Collections.Generic;

namespace IstLight.UnitTests
{
    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T,T,bool> equalsMethod;
        private readonly Func<T, int> getHashCodeMethod;

        public LambdaEqualityComparer(Func<T,T,bool> EqualsMethod, Func<T,int> GetHashCodeMethod = null)
        {
            GetHashCodeMethod = GetHashCodeMethod ?? (Func<T,int>)(x => 0);

            this.equalsMethod = EqualsMethod;
            this.getHashCodeMethod = GetHashCodeMethod;
        }

        public bool Equals(T x, T y)
        {
            return equalsMethod(x, y);
        }

        public int GetHashCode(T obj)
        {
            return getHashCodeMethod(obj);
        }
    }
}
