using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Settings
{
    public class AnnualInflationRateSetting : ISimulationSetting
    {
        public AnnualInflationRateSetting()
        {
            Value = 0;
        }

        public double Value { get; set; }

        public object Clone()
        {
            return new AnnualInflationRateSetting { Value = this.Value };
        }
    }
}
