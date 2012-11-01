using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IstLight;
using IstLight.Extensions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using System.Collections.ObjectModel;
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
