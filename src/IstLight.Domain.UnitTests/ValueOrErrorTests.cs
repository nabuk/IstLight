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

        [Fact]
        public void ctorWithFuncArg_FuncThrows_SetsError()
        {
            var ex = new InvalidOperationException();
            var sut = new ValueOrError<int>(() => { throw ex; });
            Assert.Same(ex, sut.Error);
        }

        [Fact]
        public void ctorWithFuncArg_FuncDoesNotThrow_SetsResult()
        {
            var result = 2;
            var sut = new ValueOrError<int>(() => result);
            Assert.Equal<int>(result, sut.Value);
        }

        public ValueOrError<T> CreateSut()
        {
            return new ValueOrError<T>();
        }
    }

    public class ValueOrErrorTests_Int : ValueOrErrorTests<int> { }
    public class ValueOrErrorTests_Version : ValueOrErrorTests<Version> { }
}
