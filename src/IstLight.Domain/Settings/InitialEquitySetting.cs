using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Settings
{
    public class InitialEquitySetting : ISimulationSetting
    {
        public InitialEquitySetting()
        {
            Value = 100;
        }
        
        public double Value { get; set; }

        public object Clone()
        {
            return new InitialEquitySetting { Value = this.Value };
        }
    }
}
