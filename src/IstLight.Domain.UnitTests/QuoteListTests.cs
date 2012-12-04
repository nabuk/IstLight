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

using Moq;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests
{
    
    public class QuoteListTests
    {
        [Theory]
        [QuoteListData(0, 1, 2)]
        [QuoteListData(2, 1, 0)]
        public void From_IsEarliest(QuoteList<IDate> sut)
        {
            Assert.Equal<DateTime>(sut.Min(q => q.Date), sut.From);
        }

        [Theory]
        [QuoteListData(0, 1, 2)]
        [QuoteListData(2, 1, 0)]
        public void To_IsLatest(QuoteList<IDate> sut)
        {
            Assert.Equal<DateTime>(sut.Max(q => q.Date), sut.To);
        }

        [Theory]
        [QuoteListData(0, 1, 2)]
        [QuoteListData(2, 1, 0)]
        public void From_IsFirst(QuoteList<IDate> sut)
        {
            Assert.Equal<DateTime>(sut[0].Date, sut.From);
        }

        [Theory]
        [QuoteListData(0, 1, 2)]
        [QuoteListData(2, 1, 0)]
        public void To_IsLast(QuoteList<IDate> sut)
        {
            Assert.Equal<DateTime>(sut[sut.Count-1].Date, sut.To);
        }

        [Fact]
        public void ctor_AscCollectionArgument_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new QuoteListDataAttribute(0, 1, 2).CreateCollection());
        }

        [Fact]
        public void ctor_DescCollectionArgument_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new QuoteListDataAttribute(2, 1, 0).CreateCollection());
        }

        [Fact]
        public void ctor_UnorderedCollectionArgument_Throws()
        {
            Assert.Throws<ArgumentException>(() => new QuoteListDataAttribute(1, 2, 0).CreateCollection());
        }

        [Fact]
        public void ctor_NullCollectionArgument_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new QuoteList<IDate>(null));
        }

        [Fact]
        public void ctor_EmptyCollectionArgument_Throws()
        {
            Assert.Throws<ArgumentException>(() => new QuoteListDataAttribute().CreateCollection());
        }

        [Fact]
        public void ctor_SingleElementCollection_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new QuoteListDataAttribute(0).CreateCollection());
        }

        [Theory]
        [QuoteListData(0, 1, 2)]
        [QuoteListData(2, 1, 0)]
        public void ctor_HasAscOrder(QuoteList<IDate> sut)
        {
            Assert.True(sut.SequenceEqual(sut.OrderBy(q => q.Date)));
        }

        [Fact]
        public void Count_ReturnsQuoteCount()
        {
            var sut = new QuoteListDataAttribute(0, 1, 2).CreateCollection();
            Assert.Equal<int>(3, sut.Count);
        }

        [Fact]
        public void indexer_Works()
        {
            var collection = CreateIDates(0, 1, 2).ToArray();
            var sut = new QuoteList<IDate>(collection.AsReadOnlyList());
            Assert.True(collection.SequenceEqual(sut.GetIEnumerableThroughIndexing()), "indexer does not work.");
        }

        [Fact]
        public void enumerator_Works()
        {
            var collection = CreateIDates(0, 1, 2).ToArray();
            var sut = new QuoteList<IDate>(collection.AsReadOnlyList());
            Assert.True(collection.SequenceEqual(sut), "enumerator does not work.");
        }

        [Fact]
        public void ctor_SameQuoteDates_Throws()
        {
            Assert.Throws<ArgumentException>(() => new QuoteList<IDate>(CreateIDates(2, 1, 1, 0).AsReadOnlyList()));
        }

        [Fact]
        public void ctor_NullItem_Throws()
        {
            Assert.Throws<ArgumentException>(() => new QuoteList<IDate>(CreateIDates(0).Union<IDate>(new IDate[] { null }).AsReadOnlyList()));
        }

        public IEnumerable<IDate> CreateIDates(params int[] dayOffsets)
        {
            foreach (var d in dayOffsets)
            {
                var mock = new Mock<IDate>();
                mock.Setup(x => x.Date).Returns(new DateTime(2000, 1, 1).AddDays(d));
                yield return mock.Object;
            }
        }
    }
}
