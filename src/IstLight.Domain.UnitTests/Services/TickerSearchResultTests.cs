using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Services;
using Xunit;

namespace IstLight.Domain.UnitTests.Services
{
    public class TickerSearchResultTests
    {
        [Fact]
        public void ctor_NullTickerName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new TickerSearchResult(null, "desc"));
        }

        [Fact]
        public void ctor_EmptyTickerName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new TickerSearchResult("", "desc"));
        }

        [Fact]
        public void ctor_NullTickerDescription_IsConvertedToEmptyDescription()
        {
            var sut = new TickerSearchResult("name", null);
            Assert.Equal<string>("", sut.TickerDescription);
        }

        [Fact]
        public void TickerName_FromCtorArg_IsStored()
        {
            string name = Guid.NewGuid().ToString();
            var sut = new TickerSearchResult(name, "desc");
            Assert.Equal<string>(name, sut.TickerName);
        }

        [Fact]
        public void TickerDescription_FromCtorArg_IsStored()
        {
            string desc = Guid.NewGuid().ToString();
            var sut = new TickerSearchResult("name", desc);
            Assert.Equal<string>(desc, sut.TickerDescription);
        }
    }
}
