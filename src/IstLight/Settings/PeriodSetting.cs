using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Settings
{
    public class PeriodSetting : ISimulationSetting
    {
        public PeriodSetting()
        {
            Type = PeriodType.Tick;
        }
        public PeriodType Type { get; set; }

        public object Clone()
        {
            return new PeriodSetting { Type = this.Type };
        }
    }
}
