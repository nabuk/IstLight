using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Simulation
{
    public class StrategyInfo
    {
        public StrategyInfo(Script script, string output)
        {
            this.Script = script;
            this.Output = output;
        }

        public Script Script { get; private set; }
        public string Output { get; private set; }
    }
}
