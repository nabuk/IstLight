using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Strategy
{
    public class ScriptStrategyFactory
    {
        private readonly IScriptLoadService predefinedFunctions;

        public ScriptStrategyFactory(IScriptLoadService predefinedFunctions)
        {
            this.predefinedFunctions = predefinedFunctions;
        }

        public StrategyBase CreateStrategy(Script strategyScript)
        {
            return new ScriptStrategyContext(strategyScript, predefinedFunctions);
        }
    }
}
