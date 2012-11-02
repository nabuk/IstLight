using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight;
using IstLight.Services;

namespace IstLight.Services
{
    public class ResultAnalyzerService : ScriptServiceBase<IResultAnalyzer>, IResultAnalyzerService
    {
        public ResultAnalyzerService(IScriptService scripts) : base(scripts) { }

        protected override ValueOrError<IResultAnalyzer> CreateInstance(Script script)
        {
            Exception error = null;
            var executor = new ParallelScriptExecutor(script, out error);
            if (error != null)
            {
                executor.Dispose();
                return new ValueOrError<IResultAnalyzer> { Error = error };
            }

            if (!executor.VariableExists("Analyze"))
            {
                executor.Dispose();
                return new ValueOrError<IResultAnalyzer> { Error = new ScriptException(script, "\"Analyze\" function not defined.") };
            }

            var categoryOrError = executor.SafeExecute<string>(engine => engine.GetVariable("Category"));
            if (categoryOrError.IsError)
            {
                executor.Dispose();
                return new ValueOrError<IResultAnalyzer> { Error = new ScriptException(script, "\"Category\" variable is not defined or has wrong type.") };
            }

            return new ValueOrError<IResultAnalyzer> { Value = new ResultAnalyzer(categoryOrError.Value,executor) };
        }
    }
}
