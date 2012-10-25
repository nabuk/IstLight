using System;
namespace IstLight.Domain.Settings
{
    public interface ISimulationSettings : ICloneable
    {
        T Get<T>() where T : class, ISimulationSetting, new();
    }
}
