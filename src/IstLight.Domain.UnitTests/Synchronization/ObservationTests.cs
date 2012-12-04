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
using IstLight.Synchronization;
using Xunit;

namespace IstLight.UnitTests.Synchronization
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
