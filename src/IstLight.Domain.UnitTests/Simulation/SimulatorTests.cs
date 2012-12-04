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
using IstLight.Settings;
using IstLight.Simulation;
using IstLight.Strategy;
using IstLight.Synchronization;
using Moq;
using Xunit;

namespace IstLight.UnitTests.Simulation
{
    public class SimulatorTests
    {
        [Fact]
        public void RunAsync_NullTickers_Throws()
        {
            var sut = new Simulator();
            Assert.Throws<ArgumentNullException>(() => sut.RunAsync(null, new Mock<StrategyBase>().Object, new SimulationSettings()));
        }

        [Fact]
        public void RunAsync_NullStrategy_Throws()
        {
            var sut = new Simulator();
            Assert.Throws<ArgumentNullException>(() => sut.RunAsync(CreateSyncTickersStub(), null, new SimulationSettings()));
        }

        [Fact]
        public void RunAsync_NullSettings_Throws()
        {
            var sut = new Simulator();
            Assert.Throws<ArgumentNullException>(() => sut.RunAsync(CreateSyncTickersStub(), new Mock<StrategyBase>().Object, null));
        }

        [Fact]
        public void Cancel_BeforeSimulation_DoesNothing()
        {
            var sut = new Simulator();
            Assert.DoesNotThrow(() => sut.Cancel(false));
        }

        [Fact]
        public void Cancel_AfterSimulation_DoesNothing()
        {
            var sut = new Simulator();
            sut.RunAsync(CreateSyncTickersStub(), new EmptyStrategy(), new SimulationSettings());
            sut.Wait();
            Assert.DoesNotThrow(() => sut.Cancel(false));
        }

        [Fact(Timeout=5000)]
        public void Wait_BeforeSimulation_DoesNothing()
        {
            var sut = new Simulator();
            Assert.DoesNotThrow(() => sut.Wait());
        }

        [Fact(Timeout=5000)]
        public void Wait_AfterSimulation_DoesNothing()
        {
            var sut = new Simulator();
            sut.RunAsync(CreateSyncTickersStub(), new EmptyStrategy(), new SimulationSettings());
            sut.Wait();
            Assert.DoesNotThrow(() => sut.Wait());
        }


        [Fact]
        public void IsBusyProperty_BeforeSimulation_IsFalse()
        {
            Assert.False(new Simulator().IsBusy);
        }

        [Fact]
        public void IsBusyProperty_DuringSimulation_IsTrue()
        {
            bool executionHolder = true;

            var sut = new Simulator();
            
            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Callback(() => { while (executionHolder); }).Returns(true);

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(CreateSyncTickersStub(), strategyStub.Object, settings);

            bool isBusy = sut.IsBusy;
            executionHolder = false;
            Assert.True(isBusy);
        }

        [Fact]
        public void IsBusyProperty_AfterSimulation_IsFalse()
        {
            var sut = new Simulator();

            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Returns(true);

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(CreateSyncTickersStub(), strategyStub.Object, settings);
            sut.Wait(Timeout.Infinite);
            
            Assert.False(sut.IsBusy);            
        }

        [Fact]
        public void RunAsync_IsBusyIsTrue_Thows()
        {
            bool executionHolder = true;

            var sut = new Simulator();

            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Callback(() => { while (executionHolder); }).Returns(true);

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(CreateSyncTickersStub(), strategyStub.Object, settings);

            Assert.Throws<InvalidOperationException>(() => sut.RunAsync(CreateSyncTickersStub(), strategyStub.Object, settings));
            executionHolder = false;
        }

        [Fact]
        public void ProgressChanged_DuringSimulation_Fires()
        {
            int changedEvent_FireCount = 0;
            var sut = new Simulator();
            sut.ProgressChanged += delegate { changedEvent_FireCount++; };

            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Returns(true);

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(CreateSyncTickersStub(), strategyStub.Object, settings);
            sut.Wait(Timeout.Infinite);

            Assert.True(changedEvent_FireCount > 1);
        }

        [Fact]
        public void SimulationEnded_AfterSimulation_Fires()
        {
            int endEvent_FireCount = 0;
            var sut = new Simulator();
            sut.SimulationEnded += delegate { endEvent_FireCount++; };

            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Returns(true);

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(CreateSyncTickersStub(), strategyStub.Object, settings);
            sut.Wait(Timeout.Infinite);

            Assert.Equal<int>(1, endEvent_FireCount);
        }

        [Fact]
        public void Wait_WaitsSpecifiedTimeForSimulationCompletion()
        {
            bool executionHolder = true;
            var sut = new Simulator();

            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Callback(() => { while (executionHolder);}).Returns(true);

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(CreateSyncTickersStub(), strategyStub.Object, settings);
            sut.Wait(100);

            Assert.True(sut.IsBusy);

            executionHolder = false;
        }

        [Fact]
        public void EndResult_AfterSimulation_Completion()
        {
            var sut = new Simulator();

            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Returns(true);

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(CreateSyncTickersStub(), strategyStub.Object, settings);
            sut.Wait(Timeout.Infinite);

            Assert.Equal<SimulationEndReason>(SimulationEndReason.Completion, sut.EndInfo.EndReason);
        }

        [Fact]
        public void EndResult_AfterCancelation_Cancelation()
        {
            bool executionHolder = true;
            var sut = new Simulator();

            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Callback(() => { while (executionHolder);}).Returns(true);

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(CreateSyncTickersStub(), strategyStub.Object, settings);
            sut.Cancel();

            Assert.Equal<SimulationEndReason>(SimulationEndReason.Cancellation, sut.EndInfo.EndReason);
        }

        [Fact]
        public void EndResult_AfterError_Error()
        {
            var sut = new Simulator();

            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Returns(false);
            strategyStub.Setup(x => x.LastError).Returns("error");

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(CreateSyncTickersStub(), strategyStub.Object, settings);
            sut.Wait(Timeout.Infinite);

            Assert.Equal<SimulationEndReason>(SimulationEndReason.Error, sut.EndInfo.EndReason);
        }

        [Fact]
        public void EndInfo_IsRefEqualToEndEventArgs()
        {
            SimulationEndEventArgs endInfo = null;
            var sut = new Simulator();
            sut.SimulationEnded += (s, e) => endInfo = e;

            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Returns(true);

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(CreateSyncTickersStub(), strategyStub.Object, settings);
            sut.Wait();
            Assert.Same(sut.EndInfo, endInfo);
        }

        [Fact]
        public void EndInfo_BeforeSimulation_IsNull()
        {
            Assert.Null(new Simulator().EndInfo);
        }

        [Fact]
        public void SyncTickers_InEndInfo_AreEqualToThosePassedToRunAsync()
        {
            var sut = new Simulator();
            var syncTickers = CreateSyncTickersStub();

            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Returns(true);

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(syncTickers, strategyStub.Object, settings);
            sut.Wait();
            Assert.Same(syncTickers, sut.EndInfo.Result.SyncTickers);
        }

        [Fact]
        public void Settings_InEndInfo_AreEqualToThosePassedToRunAsync()
        {
            var sut = new Simulator();
            var syncTickers = CreateSyncTickersStub();

            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Returns(true);

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(syncTickers, strategyStub.Object, settings);
            sut.Wait();
            Assert.Same(settings, sut.EndInfo.Result.Settings);
        }

        [Fact]
        public void InfinitiveSimulation_CanBeCanceled()
        {
            bool executionHolder = true;
            var sut = new Simulator();

            var strategyStub = new Mock<StrategyBase>();
            strategyStub.Setup(x => x.Initialize()).Returns(true);
            strategyStub.Setup(x => x.Run()).Callback(() => { while (executionHolder);}).Returns(true);

            var settings = new SimulationSettingsImmutableDecorator(new SimulationSettings());

            sut.RunAsync(CreateSyncTickersStub(), strategyStub.Object, settings);
            sut.Cancel();
            Assert.False(sut.IsBusy);
        }

        public static SyncTickers CreateSyncTickersStub()
        {
            return SyncTickersFactory.Synchronize(new Ticker[] { TickerTests.CreateTicker(quoteCount: 3) }.AsReadOnlyList(), new SimulationSettings());
                
        }
    }
}
