using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Extensions;

namespace IstLight.Settings
{
    public class SimulationSettingsImmutableDecorator : ISimulationSettings
    {
        private readonly ISimulationSettings settings;

        public SimulationSettingsImmutableDecorator(ISimulationSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");

            this.settings = settings;
        }

        public T Get<T>() where T : class, ISimulationSetting, new()
        {
            return this.settings.Get<T>().TypedClone();
        }

        public object Clone()
        {
            return new SimulationSettingsImmutableDecorator(settings.TypedClone());
        }
    }
}
