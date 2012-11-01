using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Strategy;
using IstLight.Synchronization;
using IstLight.Extensions;
using Moq;
using Xunit;

namespace IstLight.UnitTests.Extensions
{
    public class IQuoteContextExtensionsTests
    {
        [Fact]
        public void GetCurrentPrice_NoQuotes_ReturnsNull()
        {
            var ctxStub = new Mock<IQuoteContext>();
            ctxStub.Setup(x => x.GetQuotes(It.IsAny<int>())).Returns(new ISimpleTickerQuote[0].AsReadOnlyList());
            Assert.Null(ctxStub.Object.GetCurrentPrice(0));
        }

        [Fact]
        public void GetCurrentPrice_OneQuote_ReturnsPrice()
        {
            var quotes = new ISimpleTickerQuote[]
            {
                new SimpleTickerQuote(DateTime.Now.AddDays(0), 1, null),
            }.AsReadOnlyList();

            var ctxStub = new Mock<IQuoteContext>();
            ctxStub.Setup(x => x.GetQuotes(It.IsAny<int>())).Returns(quotes);
            Assert.Equal<double?>(quotes[0].Value,ctxStub.Object.GetCurrentPrice(0));
        }
    }
}
