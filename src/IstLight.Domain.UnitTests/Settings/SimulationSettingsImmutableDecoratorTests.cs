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
using IstLight.Settings;
using Moq;
using Xunit;

namespace IstLight.UnitTests.Settings
{
    public class SimulationSettingsImmutableDecoratorTests
    {
        [Fact]
        public void ctor_NullArg_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new SimulationSettingsImmutableDecorator(null));
        }

        [Fact]
        public void CannotModifySetting()
        {
            var sut = new SimulationSettingsImmutableDecorator(new SimulationSettings());
            var setting = sut.Get<CloseAllOnLastBarSetting>();
            bool defaultValue = setting.Value;
            setting.Value = !setting.Value;
            Assert.Equal<bool>(defaultValue, sut.Get<CloseAllOnLastBarSetting>().Value);
        }

        [Fact]
        public void Clone_InvokesCloneOnDecorated()
        {
            var settingsMock = new Mock<ISimulationSettings>();
            settingsMock.Setup(x => x.Clone()).Returns(settingsMock.Object);
            var sut = new SimulationSettingsImmutableDecorator(settingsMock.Object);
            sut.Clone();
            settingsMock.Verify(x => x.Clone(), Times.Once());
        }
    }
}
