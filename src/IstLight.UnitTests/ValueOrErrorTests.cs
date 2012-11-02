using System;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests
{
    public abstract class ValueOrErrorTests<T>
    {
        [Theory, AutoMockingData]
        public void Value_GetSetWorks(T value)
        {
            var sut = CreateSut();
            sut.Value = value;
            Assert.Equal<T>(value, sut.Value);
        }

        [Theory, AutoMockingData]
        public void Error_GetSetWorks(Exception error)
        {
            var sut = CreateSut();
            sut.Error = error;
            Assert.Equal<Exception>(error, sut.Error);
        }

        [Fact]
        public void IsError_ErrorIsNotNull_IsTrue()
        {
            var sut = CreateSut();
            sut.Error = new Exception();
            Assert.True(sut.IsError);
        }

        [Fact]
        public void IsError_ErrorIsNull_IsFalse()
        {
            var sut = CreateSut();
            sut.Error = null;
            Assert.False(sut.IsError);
        }

        public ValueOrError<T> CreateSut()
        {
            return new ValueOrError<T>();
        }
    }

    public class ValueOrErrorTests_Int : ValueOrErrorTests<int> { }
    public class ValueOrErrorTests_Version : ValueOrErrorTests<Version> { }
}
