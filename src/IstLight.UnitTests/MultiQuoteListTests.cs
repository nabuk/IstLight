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
