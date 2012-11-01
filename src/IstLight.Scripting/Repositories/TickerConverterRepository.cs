using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight;
using IstLight.Services;

namespace IstLight.Scripting.Repositories
{
    public class TickerConverterRepository : ScriptRepositoryBase<ITickerConverter>, ITickerConverterRepository
    {
        public TickerConverterRepository(IScriptRepository scripts) : base(scripts) { }

        protected override ResultOrError<ITickerConverter> CreateInstance(Script script)
        {
            Exception error = null;
            var executor = new ParallelScriptExecutor(script, out error);
            if (error != null)
            {
                executor.Dispose();
                return new ResultOrError<ITickerConverter> { Error = error };
            }

            if (!executor.VariableExists("Read"))
            {
                executor.Dispose();
                return new ResultOrError<ITickerConverter> { Error = new ScriptException(script, "\"Read\" function not defined.") };
            }

            if (!executor.VariableExists("Save"))
            {
                executor.Dispose();
                return new ResultOrError<ITickerConverter> { Error = new ScriptException(script, "\"Save\" function not defined.") };
            }

            if (!executor.VariableExists("Format"))
            {
                executor.Dispose();
                return new ResultOrError<ITickerConverter> { Error = new ScriptException(script, "\"Format\" variable not defined.") };
            }

            return new ResultOrError<ITickerConverter> { Result = new TickerConverter(executor) };
        }
    }
}
