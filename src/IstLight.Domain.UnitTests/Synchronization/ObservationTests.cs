using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using IstLight.Domain.Synchronization;
using IstLight.Domain.Extensions;

namespace IstLight.Domain.UnitTests.Synchronization
{
    public class ObservationTests
    {
        [Fact]
        public void ctor_NullQuotes_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new Observation(null, DateTime.Now));
        }

        [Fact]
        public void ctor_EmptyQuotes_Throws()
        {
            Assert.Throws<ArgumentException>(() => new Observation(new int[0].AsReadOnlyList(), DateTime.Now));
        }

        [Fact]
        public void ctor_Date_IsSet()
        {
            var date = DateTime.Now;
            var sut = new Observation(new int[1] { 1 }.AsReadOnlyList(), date);
            
            Assert.Equal<DateTime>(date, sut.Date);
        }

        [Fact]
        public void ctor_CurrentQuoteCount_IsSet()
        {
            var collection = new int[1] { 1 }.AsReadOnlyList();
            var sut = new Observation(collection, DateTime.Now);
            Assert.Same(collection, sut.CurrentQuoteCount);
        }

        [Fact]
        public void ImplementsIDate()
        {
            Assert.IsAssignableFrom<IDate>(new Observation(new int[1] { 1 }.AsReadOnlyList(), DateTime.Now));
        }

        public static Observation CreateObservation(DateTime date, params int[] currentQuoteCount)
        {
            return new Observation(currentQuoteCount.AsReadOnlyList(), date);
        }

        public class ObservationComparer : IEqualityComparer<Observation>
        {
            public bool Equals(Observation x, Observation y)
            {
                return x.CurrentQuoteCount.SequenceEqual(y.CurrentQuoteCount) && x.Date == y.Date;
            }

            public int GetHashCode(Observation obj)
            {
                return obj.Date.GetHashCode();
            }
        }
    }
}
