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

        public IAsyncResult<string> Analyze(SimulationResult result)
        {
            return executor.SafeExecuteAsync<string>(engine => engine.GetVariable("Analyze")(result));
        }
    }
}
