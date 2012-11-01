using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Services;
using IstLight.Simulation;

namespace IstLight.Scripting.Services
{
    public class ResultAnalyzer : BaseScriptServiceItem, IResultAnalyzer
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
