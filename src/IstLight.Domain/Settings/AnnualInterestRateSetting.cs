using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Settings
{
    public class AnnualInterestRateSetting : ISimulationSetting
    {
        public AnnualInterestRateSetting()
        {
            Value = 0;
        }

        public double Value { get; set; }

        public object Clone()
        {
            return new AnnualInterestRateSetting { Value = this.Value };
        }
    }
}
