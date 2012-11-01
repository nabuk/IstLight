using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Extensions;

namespace IstLight.Settings
{
    public class SimulationSettings : ICloneable, ISimulationSettings
    {
        private readonly Dictionary<Type, ISimulationSetting> settings = new Dictionary<Type, ISimulationSetting>();

        public T Get<T>() where T : class, ISimulationSetting, new()
        {
            Type settingType = typeof(T);
            ISimulationSetting result;

            if (!settings.TryGetValue(settingType, out result))
                settings.Add(settingType,result = new T());

            return result as T;
        }

        #region ICloneable
        public object Clone()
        {
            var clone = new SimulationSettings();
            foreach(var s in this.settings)
                clone.settings.Add(s.Key, s.Value.TypedClone());
            return clone;
        }
        #endregion //ICloneable
    }
}
