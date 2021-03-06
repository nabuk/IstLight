﻿// Copyright 2012 Jakub Niemyjski
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
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests
{
    public class TickerTests
    {
        [Fact]
        public void ctor_NullName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => CreateTicker(name: null));
        }

        [Fact]
        public void ctor_NullQuotes_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new Ticker("default",null));
        }

        [Fact]
        public void ctor_Name_IsRemembered()
        {
            string name = new Fixture().CreateAnonymous<string>();

            var sut = CreateTicker(name: name);

            Assert.Equal<string>(name, sut.Name);
        }

        [Fact]
        public void ctor_Name_SetterWorks()
        {
            var ticker = CreateTicker();
            string name = new Fixture().CreateAnonymous<string>();
            ticker.Name = name;

            Assert.Equal<string>(name, ticker.Name);
        }

        [Theory]
        [ClassData(typeof(CanBeBoughtTestDataProvider))]
        public void CanBeBought_IsComputedCorrectly(IReadOnlyList<TickerQuote> quotes, bool expected)
        {
            var sut = new Ticker("default", quotes);
            Assert.Equal<bool>(expected, sut.CanBeBought);
        }

        public static Ticker CreateTicker(string name = "default", int quoteCount = 2)
        {
            return CreateTickerProvidingQuotes(name, Enumerable.Range(1, quoteCount).Select(q => (double)q).ToArray());
        }
        public static Ticker CreateTickerProvidingQuotes(string name = "default", params double[] quotes)
        {
            return new Ticker(
                name,
                quotes.Select((q, i) =>
                    new TickerQuote(
                        new DateTime(2000, 1, 1).AddDays(i),
                        q,
                        1))
                .AsReadOnlyList());
        }

        public static Ticker CreateTickerByOffsets(string name = "default", params double[] offsets)
        {
            return new Ticker(
                name,
                offsets.Select(o =>
                    new TickerQuote(
                        new DateTime(2000, 1, 1).AddDays(o),
                        1,
                        1))
                .AsReadOnlyList());
        }

        public class CanBeBoughtTestDataProvider : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new TickerQuote[]
                    {
                        new TickerQuote(new DateTime(2000,1,1),1,1),
                        new TickerQuote(new DateTime(2000,1,2),1,1)
                    }.AsReadOnlyList(),
                    true
                };

                int count = 3;
                for (int lowIndex = 0; lowIndex < count; lowIndex++)
                    for (int dayOffset = -1; dayOffset == -1 || dayOffset == 1; dayOffset += 2)
                        for (double lowPrice = -1; lowPrice <= 0; lowPrice += 1)
                            yield return new object[] { CreateQuotes(dayOffset, lowIndex, lowPrice, count), false };
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private IReadOnlyList<ITickerQuote> CreateQuotes(int dayOffset, int lowIndex, double lowPrice, int count)
            {
                return Enumerable.Range(0, count).Select(i =>
                    new TickerQuote(
                        new DateTime(2000, 1, 1).AddDays(dayOffset+i),
                        1,
                        1,
                        5,
                        i == lowIndex ? lowPrice : 1,
                        1))
                .AsReadOnlyList();
            }
        }
    }
}
