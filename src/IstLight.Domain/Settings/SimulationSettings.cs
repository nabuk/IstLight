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
