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
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests
{
    public class MultiQuoteListTests
    {
        [Fact]
        public void ctor_NullQuotes_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new MultiQuoteList<IDate>(null, CreateDescriptions()));
        }

        [Fact]
        public void ctor_NullDescriptions_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new MultiQuoteList<IDate>(new QuoteListDataAttribute(1,2).CreateCollection(), null));
        }

        [Fact]
        public void Descriptions_ReturnsPassedToCtorArg()
        {
            var descriptions = CreateDescriptions();
            var sut = new MultiQuoteList<IDate>(new QuoteListDataAttribute(1, 2).CreateCollection(), descriptions);

            Assert.True(object.ReferenceEquals(descriptions, sut.Descriptions));
        }

        [Fact]
        public void TickerCount_ReturnsUnderlyingTickerCount()
        {
            var descriptions = CreateDescriptions();
            var sut = new MultiQuoteList<IDate>(new QuoteListDataAttribute(1, 2).CreateCollection(), descriptions);
            Assert.Equal<int>(descriptions.Count, sut.TickerCount);
        }

        [Theory]
        [InlineData(3,0)]
        [InlineData(3,1)]
        [InlineData(3,2)]
        public void TickerIndexByName_GivenValidName_ReturnsCorrectIndex(int descriptionCount, int lookForIndex)
        {
            var descriptions = CreateDescriptions(descriptionCount);
            string lookForName = descriptions[lookForIndex].Name;

            var sut = new MultiQuoteList<IDate>(new QuoteListDataAttribute(1, 2).CreateCollection(), descriptions);

            Assert.Equal<int?>(lookForIndex, sut.TickerIndexByName(lookForName));
        }

        [Fact]
        public void TickerIndexByName_GivenInvalidName_ReturnsNull()
        {
            var sut = new MultiQuoteList<IDate>(new QuoteListDataAttribute(1, 2).CreateCollection(), CreateDescriptions());

            Assert.Null(sut.TickerIndexByName("__invalid name__"));
        }

        public IReadOnlyList<TickerDescription> CreateDescriptions(int count = 3)
        {
            var fixture = new Fixture();
            fixture.RepeatCount = count;
            return fixture.CreateMany<TickerDescription>().AsReadOnlyList();
        }
    }
}
