using System;
using IstLight.Services;
using Xunit;

namespace IstLight.UnitTests.Services
{
    public class TickerSearchResultTests
    {
        [Fact]
        public void ctor_NullName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new TickerSearchResult(null, "desc", "usa"));
        }

        [Fact]
        public void ctor_EmptyName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new TickerSearchResult("", "desc", "usa"));
        }

        [Fact]
        public void ctor_NullDescription_IsConvertedToEmpty()
        {
            var sut = new TickerSearchResult("name", null, "usa");
            Assert.Equal<string>("", sut.Description);
        }

        [Fact]
        public void ctor_NullMarket_IsConvertedToEmpty()
        {
            var sut = new TickerSearchResult("name", "desc", null);
            Assert.Equal<string>("", sut.Market);
        }

        [Fact]
        public void Name_FromCtorArg_IsStored()
        {
            string name = Guid.NewGuid().ToString();
            var sut = new TickerSearchResult(name, "desc", "usa");
            Assert.Equal<string>(name, sut.Name);
        }

        [Fact]
        public void Description_FromCtorArg_IsStored()
        {
            string desc = Guid.NewGuid().ToString();
            var sut = new TickerSearchResult("name", desc, "usa");
            Assert.Equal<string>(desc, sut.Description);
        }

        [Fact]
        public void Market_FromCtorArg_IsStored()
        {
            string market = Guid.NewGuid().ToString();
            var sut = new TickerSearchResult("name", "desc", market);
            Assert.Equal<string>(market, sut.Market);
        }
    }
}
