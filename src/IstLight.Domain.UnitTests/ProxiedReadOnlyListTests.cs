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
using System.Collections.ObjectModel;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests
{
    public abstract class ProxiedReadOnlyListTests<T>
    {
        [Fact]
        public void ctor_NullIndexer_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ProxiedReadOnlyList<T>(indexer:null, countGetter:() => 3));
        }

        [Fact]
        public void ctor_NullCountGetter_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ProxiedReadOnlyList<T>(indexer: (i => default(T)), countGetter: null));
        }

        [Fact]
        public void indexer_Works()
        {
            var inputList = new List<T>();
            new Fixture().Customize(new AutoMoqCustomization()).AddManyTo(inputList);
            var readonlyListArgument = new ReadOnlyCollection<T>(inputList);

            var sut = new ProxiedReadOnlyList<T>(i => readonlyListArgument[i], () => readonlyListArgument.Count);

            Assert.True(readonlyListArgument.SequenceEqual(sut.GetIEnumerableThroughIndexing()), "indexer does not work.");
        }

        [Fact]
        public void enumerator_Works()
        {
            var inputList = new List<T>();
            new Fixture().Customize(new AutoMoqCustomization()).AddManyTo(inputList);
            var readonlyListArgument = new ReadOnlyCollection<T>(inputList);

            var sut = new ProxiedReadOnlyList<T>(i => readonlyListArgument[i], () => readonlyListArgument.Count);

            Assert.True(readonlyListArgument.SequenceEqual(sut), "enumerator does not work.");
        }

        [Theory, AutoMockingData]
        public void ImplementsIEnumerable(ProxiedReadOnlyList<T> sut)
        {
            Assert.IsAssignableFrom<IEnumerable<T>>(sut);
        }

        [Theory, AutoMockingData]
        public void ImplementsIReadOnlyList(ProxiedReadOnlyList<T> sut)
        {
            Assert.IsAssignableFrom<IReadOnlyList<T>>(sut);
        }

        [Theory]
        [InlineData(5, 5)]
        [InlineData(0, 0)]
        [InlineData(-5, 0)]
        public void Count_ReturnsCorrectNonNegativeValue(int passedCount, int expected)
        {
            var sut = new ProxiedReadOnlyList<T>(i => default(T), () => passedCount);

            Assert.Equal<int>(expected, sut.Count);
        }

        [Theory]
        [InlineData(5, 5)]
        [InlineData(0, 0)]
        [InlineData(-5, 0)]
        [InlineData(2, 4)]
        [InlineData(4, -4)]
        [InlineData(0, -2)]
        public void indexer_IndexOutOfRange_Throws(int passedCount, int requestedIndex)
        {
            var sut = new ProxiedReadOnlyList<T>(i => default(T), () => passedCount);

            Assert.Throws<IndexOutOfRangeException>(() => sut[requestedIndex]);
        }


        
    }

    public class ProxiedReadOnlyListTests_Int32 : ProxiedReadOnlyListTests<Int32> { };
    public class ProxiedReadOnlyListTests_String : ProxiedReadOnlyListTests<String> { };
    public class ProxiedReadOnlyListTests_ITickerQuote : ProxiedReadOnlyListTests<ITickerQuote> { };
    public class ProxiedReadOnlyListTests_Version : ProxiedReadOnlyListTests<Version> { };
}
