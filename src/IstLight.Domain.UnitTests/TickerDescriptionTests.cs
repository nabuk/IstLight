using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;
using IstLight.Domain.Extensions;
using Ploeh.AutoFixture;

namespace IstLight.Domain.UnitTests
{
    public class TickerDescriptionTests
    {
        [Theory, AutoMockingData]
        public void NameProperty_GetSetWorks(string value, TickerDescription sut)
        {
            sut.Name = value;
            Assert.Equal<string>(value, sut.Name);
        }

        [Theory]
        [InlineAutoData(true)]
        [InlineAutoData(false)]
        public void CanBeBoughtProperty_GetSetWorks(bool value, TickerDescription sut)
        {
            sut.CanBeBought = value;
            Assert.Equal<bool>(value, sut.CanBeBought);
        }

        [Fact]
        public void GetDescription_HasCorrectName()
        {
            string tickerName = new Fixture().CreateAnonymous<string>();
            var ticker = TickerTests.CreateTicker(name: tickerName);
            var sut = ticker.GetDescription();

            Assert.Equal<string>(tickerName, sut.Name);
        }

        [Fact]
        public void GetDescription_ChangingTickerName_AffectsDescription()
        {
            var ticker = TickerTests.CreateTicker();
            string newTickerName = new Fixture().CreateAnonymous<string>();
            ticker.Name = newTickerName;
            var sut = ticker.GetDescription();

            Assert.Equal<string>(newTickerName, sut.Name);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetDescription_HasCorrectCanBeBought(bool expected)
        {
            var ticker = TickerTests.CreateTickerProvidingQuotes(quotes: expected ? new double[] { 2, 1 } : new double[] { 1, 0 });
            var sut = ticker.GetDescription();

            Assert.Equal<bool>(expected, sut.CanBeBought);
        }

        public class TickerDescriptionComparer : IEqualityComparer<TickerDescription>
        {
            public bool Equals(TickerDescription x, TickerDescription y)
            {
                return string.Equals(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(TickerDescription obj)
            {
                return obj.Name.GetHashCode();
            }
        }
    }
}
