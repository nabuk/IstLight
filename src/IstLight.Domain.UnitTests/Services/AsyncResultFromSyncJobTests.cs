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
using System.Threading;
using IstLight.Services;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;

namespace IstLight.UnitTests.Services
{
    public abstract class AsyncResultFromSyncJobTests<T>
    {
        [Fact(Timeout=5000)]
        public void ctor_NullArg_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => CreateSut(null));
        }

        [Fact(Timeout = 5000)]
        public void Result_WhenPassedJobReturnsError_IsDefault()
        {
            var sut = CreateErrorSut(new Exception());
            sut.Wait();
            Assert.Equal<T>(default(T), sut.Result);
        }

        [Fact(Timeout = 5000)]
        public void Result_WhenPassedJobSucceeded_ReturnsJobResult()
        {
            var result = Whatever;
            var sut = CreateCorrectSut(result);
            sut.Wait();
            Assert.Equal<T>(result, sut.Result);
        }

        [Fact(Timeout = 5000)]
        public void Error_WhenPassedJobReturnsError_ReturnsJobError()
        {
            var error = new Exception();
            var sut = CreateErrorSut(error);
            sut.Wait();
            Assert.Same(error, sut.Error);
        }

        [Fact(Timeout = 5000)]
        public void Error_WhenPassedJobSucceeded_IsNull()
        {
            var sut = CreateCorrectSut(Whatever);
            sut.Wait();
            Assert.Null(sut.Error);
        }

        [Fact(Timeout = 5000)]
        public void IsCompleted_WhenJobIsRunning_IsFalse()
        {
            bool isCompleted;
            var resume = new ManualResetEvent(false);
            
            var sut = CreateSut(() =>
                {
                    resume.WaitOne();
                    resume.Dispose();
                    return new ValueOrError<T> { Value = Whatever };
                });
            isCompleted = sut.IsCompleted;
            resume.Set();
            
            Assert.False(isCompleted);
        }

        [Fact(Timeout = 5000)]
        public void IsCompleted_WhenJobEnded_IsTrue()
        {
            var sut = CreateCorrectSut(Whatever);
            sut.Wait();
            Assert.True(sut.IsCompleted);
        }

        [Fact(Timeout = 5000)]
        public void Wait_WaitsForJobEnd()
        {
            var sut = CreateCorrectSut(Whatever);
            Assert.True(sut.Wait());
        }

        [Fact(Timeout = 5000)]
        public void Wait_NotEnoughTimeout_DoesNotWaitForJobEnd()
        {
            bool waitStatus;
            var resume = new ManualResetEvent(false);

            var sut = CreateSut(() =>
                {
                    resume.WaitOne();
                    resume.Dispose();
                    return new ValueOrError<T> { Value = Whatever };
                });
                waitStatus = sut.Wait(1);
                resume.Set();

            Assert.False(waitStatus);
        }

        [Fact(Timeout = 5000)]
        public void AddCallback_NullCallback_Throws()
        {
            var sut = CreateCorrectSut(Whatever);
            Assert.Throws<ArgumentNullException>(() => sut.AddCallback(null));
        }

        [Fact(Timeout = 5000)]
        public void AddCallback_AddedBeforeJobEnd_CallbackWillBeExecuted()
        {
            var callbackExecuted = new ManualResetEvent(false);

            var resume = new ManualResetEvent(false);

            var sut = CreateSut(() =>
            {
                resume.WaitOne();
                resume.Dispose();
                return new ValueOrError<T> { Value = Whatever };
            });
            sut.AddCallback(x => callbackExecuted.Set());
            resume.Set();

            Assert.True(callbackExecuted.WaitOne());
            callbackExecuted.Dispose();
        }

        [Fact(Timeout = 5000)]
        public void AddCallback_AddedAfterJobEnd_CallbackWillBeExecuted()
        {
            var callbackExecuted = new ManualResetEvent(false);
            var sut = CreateCorrectSut(Whatever);
            sut.Wait();
            sut.AddCallback(x => callbackExecuted.Set());
            Assert.True(callbackExecuted.WaitOne());
            callbackExecuted.Dispose();
        }

        [Fact(Timeout = 5000)]
        public void Implements_IAsyncResultOfT()
        {
            Assert.IsAssignableFrom<IAsyncResult<T>>(CreateCorrectSut(Whatever));
        }

        public AsyncResultFromSyncJob<T> CreateSut(Func<ValueOrError<T>> job)
        {
            return new AsyncResultFromSyncJob<T>(job);
        }

        public AsyncResultFromSyncJob<T> CreateCorrectSut(T result)
        {
            return CreateSut(() => new ValueOrError<T>(() => result));
        }

        public AsyncResultFromSyncJob<T> CreateErrorSut(Exception ex)
        {
            return CreateSut(() => new ValueOrError<T> { Error = ex });
        }

        public T Whatever
        {
            get { return new Fixture().Customize(new AutoMoqCustomization()).CreateAnonymous<T>(); }
        }
    }

    public class AsyncResultFromSyncJobTests_Int32 : AsyncResultFromSyncJobTests<int> { }
    public class AsyncResultFromSyncJobTests_Version : AsyncResultFromSyncJobTests<Version> { }
}
