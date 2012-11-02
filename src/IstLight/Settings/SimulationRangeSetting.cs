using System;

namespace IstLight.Settings
{
    public class SimulationRangeSetting : ISimulationSetting
    {
        public SimulationRangeSetting()
        {
            Type = SimulationRangeType.Common;
        }

        public SimulationRangeType Type { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public object Clone()
        {
            return new SimulationRangeSetting { From = this.From, To = this.To, Type = this.Type };
        }
    }
}
