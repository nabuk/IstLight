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

using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;

namespace IstLight.UnitTests.Extensions
{
    public abstract class IReadOnlyListExtensionsTests<T>
    {
        [Fact]
        public void AsReadOnlyList_IList_ReturnsEqualCollection()
        {
            IList<T> inputList = new List<T>();
            new Fixture().Customize(new AutoMoqCustomization()).AddManyTo(inputList);

            var sut = inputList.AsReadOnlyList();

            Assert.True(inputList.SequenceEqual(sut));
        }

        [Fact]
        public void AsReadOnlyList_Array_ReturnsEqualCollection()
        {
            IList<T> listBuffer = new List<T>();
            new Fixture().Customize(new AutoMoqCustomization()).AddManyTo(listBuffer);
            var array = listBuffer.ToArray();

            var sut = array.AsReadOnlyList();

            Assert.True(array.SequenceEqual(sut));
        }

        [Fact]
        public void AsReadOnlyList_IEnumerable_ReturnsEqualCollection()
        {
            IList<T> listBuffer = new List<T>();
            new Fixture().Customize(new AutoMoqCustomization()).AddManyTo(listBuffer);

            var sut = IReadOnlyListExtensions.AsReadOnlyList((IEnumerable<T>)listBuffer);

            Assert.True(listBuffer.SequenceEqual(sut));
        }

        [Fact]
        public void Reverse_IReadOnlyList_Reverses()
        {
            IList<T> listBuffer = new List<T>();
            new Fixture().Customize(new AutoMoqCustomization()).AddManyTo(listBuffer);

            var sut = listBuffer.AsReadOnlyList().Reverse();

            Assert.True(listBuffer.Reverse().SequenceEqual(sut));
        }
    }

    public class IReadOnlyListExtensionsTests_Int32 : IReadOnlyListExtensionsTests<Int32> { };
    public class IReadOnlyListExtensionsTests_String : IReadOnlyListExtensionsTests<String> { };
    public class IReadOnlyListExtensionsTests_ITickerQuote : IReadOnlyListExtensionsTests<ITickerQuote> { };
    public class IReadOnlyListExtensionsTests_Version : IReadOnlyListExtensionsTests<Version> { };
}
