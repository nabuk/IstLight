using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Settings;
using Moq;
using Xunit;

namespace IstLight.Domain.UnitTests.Settings
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
