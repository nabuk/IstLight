using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Settings
{
    public class TradeDelaySetting : ISimulationSetting
    {
        public TradeDelaySetting()
        {
            Value = 1;
        }

        public int Value { get; set; }

        public object Clone()
        {
            return new TradeDelaySetting { Value = this.Value };
        }
    }
}
