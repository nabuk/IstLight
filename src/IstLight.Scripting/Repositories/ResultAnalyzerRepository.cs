using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain;
using IstLight.Domain.Services;

namespace IstLight.Scripting.Repositories
{
    public class ResultAnalyzerRepository : ScriptRepositoryBase<IResultAnalyzer>, IResultAnalyzerRepository
    {
        public ResultAnalyzerRepository(IScriptRepository scripts) : base(scripts) { }

        protected override ResultOrError<IResultAnalyzer> CreateInstance(Script script)
        {
            Exception error = null;
            var executor = new ParallelScriptExecutor(script, out error);
            if (error != null)
            {
                executor.Dispose();
                return new ResultOrError<IResultAnalyzer> { Error = error };
            }

            if (!executor.VariableExists("Analyze"))
            {
                executor.Dispose();
                return new ResultOrError<IResultAnalyzer> { Error = new ScriptException(script, "\"Analyze\" function not defined.") };
            }

            if (!executor.VariableExists("Category"))
            {
                executor.Dispose();
                return new ResultOrError<IResultAnalyzer> { Error = new ScriptException(script, "\"Category\" variable not defined.") };
            }

            return new ResultOrError<IResultAnalyzer> { Result = new ResultAnalyzer(executor) };
        }
    }
}
