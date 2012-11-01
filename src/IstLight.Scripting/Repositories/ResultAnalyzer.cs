using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Services;
using IstLight.Simulation;

namespace IstLight.Scripting.Repositories
{
    public class ResultAnalyzer : BaseScriptRepositoryItem, IResultAnalyzer
    {
        public ResultAnalyzer(ParallelScriptExecutor executor) : base(executor) { }

        public string Category
        {
            get
            {
                var resultOrError = executor.SafeExecute<string>(engine => engine.GetVariable("Category"));
                if (resultOrError.Error != null) throw resultOrError.Error;
                return resultOrError.Result;
            }
        }

        public IAsyncResult<string> Analyze(SimulationResult result)
        {
            return executor.SafeExecuteAsync<string>(engine => engine.GetVariable("Analyze")(result));
        }
    }
}
