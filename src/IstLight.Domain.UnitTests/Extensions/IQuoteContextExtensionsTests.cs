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
using IstLight.Strategy;
using IstLight.Synchronization;
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
