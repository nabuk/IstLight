using System.Collections.Generic;
using System.Linq;
using ScriptingWrapper;

namespace IstLight.Strategy
{
    public class ScriptStrategyContext : StrategyBase
    {
        private readonly Script strategyScript;
        private readonly IScriptService predefinedFunc;
        private ScriptEngineBase engine;

        public ScriptStrategyContext(Script strategyScript, IScriptService predefinedFunc)
        {
            this.strategyScript = strategyScript;
            this.predefinedFunc = predefinedFunc;
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

            foreach (var funcGroup in predefinedFunc.Load().GroupBy(x => x.Extension))
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

                    items = engine.GetItems().ToArray();
                }

            foreach (var item in items)
                engine.SetVariable(item.Key, item.Value);

            if (!engine.SetScript(strategyScript.Content))
                return false;

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
