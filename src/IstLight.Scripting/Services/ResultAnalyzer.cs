using System.Collections.Generic;
using IstLight.Simulation;

namespace IstLight.Services
{
    public class ResultAnalyzer : ScriptNamedItemBase, IResultAnalyzer
    {
        public ResultAnalyzer(string category, ParallelScriptExecutor executor) : base(executor)
        {
            this.Category = category;
        }

        public string Category
        {
            get; private set;
        }

        public IAsyncResult<IReadOnlyList<KeyValuePair<string, string>>> Analyze(SimulationResult result)
        {
            return executor.SafeExecuteAsync<IReadOnlyList<KeyValuePair<string, string>>>(engine =>
                ((KeyValuePair<string,string>[])engine.GetVariable("Analyze")(result)).AsReadOnlyList());
        }
    }
}
