using System;
namespace IstLight.Settings
{
    public interface ISimulationSettings : ICloneable
    {
        T Get<T>() where T : class, ISimulationSetting, new();
    }
}
