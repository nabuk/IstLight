using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Strategy;
using ScriptingWrapper;

namespace IstLight.Scripting.Strategy
{
    public class ScriptStrategyContext : StrategyBase
    {
        private readonly Script script;
        private ScriptEngineBase engine;

        public ScriptStrategyContext(Script script)
        {
            this.script = script;
        }

        public override bool Initialize()
        {
            if ((engine = ScriptEngineFactory.TryCreateEngine(script.Extension)) == null)
            {
                lastError = string.Format("Script extension \"{0}\" is not valid.", script.Extension);
                return false;
            }

            if (!engine.SetScript(script.Content))
                return false;

            engine.SetVariable("quotes", base.QuoteContext);
            engine.SetVariable("wallet", base.WalletContext);

            return false;
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
