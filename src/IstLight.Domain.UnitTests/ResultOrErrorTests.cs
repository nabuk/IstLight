using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Services;
using Xunit;
using Ploeh.AutoFixture;
using Xunit.Extensions;

namespace IstLight.Domain.UnitTests
{
    public abstract class ResultOrErrorTests<T>
    {
        [Theory, AutoMockingData]
        public void Result_GetSetWorks(T result)
        {
            var sut = CreateSut();
            sut.Result = result;
            Assert.Equal<T>(result, sut.Result);
        }

        [Theory, AutoMockingData]
        public void Error_GetSetWorks(Exception error)
        {
            var sut = CreateSut();
            sut.Error = error;
            Assert.Equal<Exception>(error, sut.Error);
        }

        public ResultOrError<T> CreateSut()
        {
            return new ResultOrError<T>();
        }
    }

    public class ResultOrErrorTests_Int : ResultOrErrorTests<int> { }
    public class ResultOrErrorTests_Version : ResultOrErrorTests<Version> { }
}
