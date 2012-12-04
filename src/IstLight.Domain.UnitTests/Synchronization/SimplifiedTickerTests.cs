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
    public class SimplifiedTickerTests
    {
        [Fact]
        public void ctor_NullQuotes_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new SimplifiedTicker(null));
        }
        [Fact]
        public void ctor_EmptyQuotes_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new SimplifiedTicker(new ISimpleTickerQuote[0].AsReadOnlyList()));
        }

        [Fact]
        public void Count_ctorNullQuotes_ReturnsZero()
        {
            var sut = new SimplifiedTicker(null);
            Assert.Equal<int>(0, sut.Count);
        }
        
        [Fact]
        public void Count_Works()
        {
            var quotes = CreateQuotes(0, 1, 2).ToArray();
            var sut = new SimplifiedTicker(quotes.AsReadOnlyList());
            Assert.Equal<int>(3, sut.Count);
        }

        [Fact]
        public void indexer_Works()
        {
            var quotes = CreateQuotes(0, 1, 2).ToArray();
            var sut = new SimplifiedTicker(quotes.AsReadOnlyList());
            Assert.True(
                quotes.SequenceEqual(sut.GetIEnumerableThroughIndexing()),
                "indexer does not work.");
        }

        [Fact]
        public void enumerator_Works()
        {
            var quotes = CreateQuotes(0, 1, 2).ToArray();
            var sut = new SimplifiedTicker(quotes.AsReadOnlyList());
            Assert.True(quotes.SequenceEqual(sut), "enumerator does not work.");
        }

        public static SimplifiedTicker CreateTicker(params int[] dayOffsets)
        {
            return new SimplifiedTicker(CreateQuotes(dayOffsets).AsReadOnlyList());
        }

        public static IEnumerable<ISimpleTickerQuote> CreateQuotes(params int[] dayOffsets)
        {
            foreach (var offset in dayOffsets)
                yield return new SimpleTickerQuote(new DateTime(2000, 1, 1).AddDays(offset), 1, 1);
        }

        public class SimplifiedTickerComparer : IEqualityComparer<SimplifiedTicker>
        {
            public bool Equals(SimplifiedTicker x, SimplifiedTicker y)
            {
                return x.Count == y.Count && x.Zip(y, (q1, q2) => (q1.Date == q2.Date && q1.Value == q2.Value && q1.Volume == q2.Volume)).All(b => b);
            }

            public int GetHashCode(SimplifiedTicker obj)
            {
                return base.GetHashCode();
            }
        }
    }
}
