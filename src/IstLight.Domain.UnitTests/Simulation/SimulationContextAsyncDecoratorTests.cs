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
using System.Threading;
using System.Threading.Tasks;
using IstLight.Simulation;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace IstLight.UnitTests.Simulation
{
    public class SimulationContextAsyncDecoratorTests
    {
        [Fact]
        public void ctor_NullArg_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new SimulationContextAsyncDecorator(null));
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void BeginStep_IsCalledOnDecorated(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            sut.BeginStep();
            ctx.Verify(x => x.BeginStep(), Times.Once());
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void EndStep_IsCalledOnDecorated(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            sut.EndStep();
            ctx.Verify(x => x.EndStep(), Times.Once());
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void ProcessPendingTransactions_IsCalledOnDecorated(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            sut.ProcessPendingTransactions();
            ctx.Verify(x => x.ProcessPendingTransactions(), Times.Once());
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void ApplyInteresetRate_IsCalledOnDecorated(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            sut.ApplyInterestRate();
            ctx.Verify(x => x.ApplyInterestRate(), Times.Once());
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void GetResult_IsCalledOnDecorated(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            sut.GetResult();
            ctx.Verify(x => x.GetResult(), Times.Once());
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void GetLastError_IsCalledOnDecorated(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            sut.GetLastError();
            ctx.Verify(x => x.GetLastError(), Times.Once());
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void Initialize_IsCalledOnDecorated(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            sut.Initialize(null, null, null);
            ctx.Verify(x => x.Initialize(null,null,null), Times.Once());
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void Dispose_IsCalledOnDecorated(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            sut.Dispose();
            ctx.Verify(x => x.Dispose(), Times.Once());
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void RunStrategy_IsCalledOnDecorated(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            sut.Initialize(null, null, null);
            sut.RunStrategy();
            ctx.Verify(x => x.RunStrategy(), Times.Once());
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void RunStrategy_CorrectStrategy_ReturnsTrue(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            ctx.Setup(x => x.RunStrategy()).Returns(true);
            sut.Initialize(null, null, null);
            Assert.True(sut.RunStrategy());
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void RunStrategy_IncorrectStrategy_ReturnsFalse(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            ctx.Setup(x => x.RunStrategy()).Returns(false);
            sut.Initialize(null, null, null);
            Assert.False(sut.RunStrategy());
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void RunStrategy_CanceledBeforeRun_ReturnsFalse(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            ctx.Setup(x => x.RunStrategy()).Returns(true);
            sut.Initialize(null, null, null);
            (sut as SimulationContextAsyncDecorator).Cancel();
            Assert.False(sut.RunStrategy());
        }

        [Theory(Timeout = 30000), PropertyData("DecoratorAndMock")]
        public void RunStrategy_CanceledAfterRun_ReturnsFalse(ISimulationContext sut, Mock<ISimulationContext> ctx)
        {
            ctx.Setup(x => x.RunStrategy()).Callback(() => Thread.Sleep(60000)).Returns(true);
            sut.Initialize(null, null, null);
            Task.Factory.StartNew(() => { Thread.Sleep(1000);(sut as SimulationContextAsyncDecorator).Cancel();});
            bool result = sut.RunStrategy();
            Assert.False(result);
        }


        public static IEnumerable<object[]> DecoratorAndMock
        {
            get
            {
                var ctxMock = new Mock<ISimulationContext>();
                var sut = new SimulationContextAsyncDecorator(ctxMock.Object);
                yield return new object[] { sut, ctxMock };
            }
        }
        
    }
}
