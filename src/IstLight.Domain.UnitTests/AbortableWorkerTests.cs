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
using Moq;
using Xunit;

namespace IstLight.UnitTests
{
    public class AbortableWorkerTests
    {
        [Fact]
        public void ctor_NullExternalWork_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new AbortableWorker(null));
        }

        [Fact(Timeout = 30000)]
        public void WorkDelegate_IsNotCalledBeforeDoWork()
        {
            var workMock = PrepareExternalWorkMock();
            using (var sut = new AbortableWorker(workMock.Object.DoWork))
            {
                sut.Start();
                while (!sut.IsWaiting) Thread.Sleep(10);
            }
            workMock.Verify(x => x.DoWork(), Times.Never());
        }

        [Fact(Timeout = 30000)]
        public void WorkDelegate_IsCalledOnceOnDoWork()
        {
            var workMock = PrepareExternalWorkMock();
            using (var sut = new AbortableWorker(workMock.Object.DoWork))
            {
                sut.Start();
                sut.ResumeWork();
                sut.WaitForWorkDone(Timeout.Infinite);
            }
            workMock.Verify(x => x.DoWork(), Times.Once());
        }

        [Fact(Timeout = 30000)]
        public void OnDoWorkFalse_Aborts()
        {
            var workMock = PrepareExternalWorkMock(0);
            using (var sut = new AbortableWorker(workMock.Object.DoWork))
            {
                sut.Start();
                sut.ResumeWork();
                sut.WaitForWorkDone(Timeout.Infinite);

                Assert.True(sut.ThreadInstance.Join(Timeout.Infinite));
            }
        }

        [Fact(Timeout = 30000)]
        public void Abort_WithoutTimeout_AbortsWithoutException()
        {
            using (var sut = new AbortableWorker(() => { Thread.Sleep(10000); return true; }))
            {
                sut.Start();
                sut.ResumeWork();
                sut.Abort();
                sut.ThreadInstance.Join();

                Assert.True(sut.Aborted);
            }
        }

        [Fact(Timeout = 30000)]
        public void Abort_WithTimeout_AbortsWithoutException()
        {
            using (var sut = new AbortableWorker(() => { Thread.Sleep(60000); return true; }))
            {
                sut.Start();
                sut.ResumeWork();
                sut.Abort(400);
                sut.ThreadInstance.Join();

                Assert.True(sut.Aborted);
            }
        }

        [Fact(Timeout = 30000)]
        public void LastResult_ExternalWorkReturnedTrue_IsTrue()
        {
            using (var sut = new AbortableWorker(() => true))
            {
                sut.Start();
                sut.ResumeWork();
                sut.WaitForWorkDone();
                Assert.True(sut.LastRunResult);
            }
        }

        [Fact(Timeout = 30000)]
        public void LastResult_ExternalWorkReturnedFalse_IsFalse()
        {
            using (var sut = new AbortableWorker(() => false))
            {
                sut.Start();
                sut.ResumeWork();
                sut.WaitForWorkDone();
                Assert.False(sut.LastRunResult);
            }
        }

        [Fact(Timeout = 30000)]
        public void LastResult_BeforeExternalWork_IsFalse()
        {
            using (var sut = new AbortableWorker(() => true))
            {
                sut.Start();
                Assert.False(sut.LastRunResult);
            }
        }

        private Mock<IExternalWork> PrepareExternalWorkMock(int returnTrueCount = int.MaxValue)
        {
            var externalWorkMock = new Mock<IExternalWork>();
            externalWorkMock
                .Setup(x => x.DoWork())
                .Returns(() => returnTrueCount > 0)
                .Callback(() => returnTrueCount--);
            return externalWorkMock;
        }


        public interface IExternalWork
        {
            bool DoWork();
        }
    }
}
