using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Settings
{
    public class CommissionSetting : ISimulationSetting
    {
        public CommissionSetting()
        {
            Value = 0;
            Type = CommissionType.Percent;
        }

        public double Value { get; set; }
        public CommissionType Type { get; set; }

        public object Clone()
        {
            return new CommissionSetting { Value = this.Value, Type = this.Type };
        }
    }
}
