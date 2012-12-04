// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Linq;
using ScriptingWrapper;

namespace IstLight.Strategy
{
    public class ScriptStrategyContext : StrategyBase
    {
        private readonly Script strategyScript;
        private readonly IScriptLoadService predefinedFunctions;
        private ScriptEngineBase engine;

        public ScriptStrategyContext(Script strategyScript, IScriptLoadService predefinedFunctions)
        {
            this.strategyScript = strategyScript;
            this.predefinedFunctions = predefinedFunctions;
        }

        public override bool Initialize()
        {
            if ((engine = ScriptEngineFactory.TryCreateEngine(strategyScript.Extension)) == null)
            {
                lastError = string.Format("Script extension \"{0}\" is not valid.", strategyScript.Extension);
                return false;
            }

            var items = new KeyValuePair<string,dynamic>[]
            {
                new KeyValuePair<string,dynamic>("__quotes__", base.QuoteContext),
                new KeyValuePair<string,dynamic>("__wallet__", base.WalletContext)
            };

            foreach (var funcGroup in predefinedFunctions.Load().GroupBy(x => x.Extension))
                using (var groupEngine = ScriptEngineFactory.TryCreateEngine(funcGroup.Key))
                {
                    foreach (var item in items)
                        groupEngine.SetVariable(item.Key, item.Value);

                    foreach (var funcScript in funcGroup)
                    {
                        if (!groupEngine.SetScript(funcScript.Content) || !groupEngine.Execute())
                        {
                            lastError = string.Format("Predefined script {0}.{1} error: {2}", funcScript.Name, funcScript.Extension, groupEngine.LastError);
                            return false;
                        }
                    }

                    items = groupEngine.GetItems().ToArray();
                }

            foreach (var item in items)
                engine.SetVariable(item.Key, item.Value);

            if (!engine.SetScript(strategyScript.Content))
                return false;

            return true;
        }

        public override bool Run()
        {
            return engine.Execute();
        }

        public override void Dispose()
        {
            if (engine != null) engine.Dispose();
            base.Dispose();
        }

        private string lastError;
        public override string LastError { get { return lastError ?? engine.LastError; } }
    }
}
