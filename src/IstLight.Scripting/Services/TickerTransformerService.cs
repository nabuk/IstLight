using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight;
using IstLight.Services;


namespace IstLight.Scripting.Services
{
    public class TickerTransformerService : ScriptServiceBase<ITickerTransformer>, ITickerTransformerService
    {
        public TickerTransformerService(IScriptService scripts) : base(scripts) { }

        protected override ValueOrError<ITickerTransformer> CreateInstance(Script script)
        {
            Exception error = null;
            var executor = new ParallelScriptExecutor(script, out error);
            if (error != null)
            {
                executor.Dispose();
                return new ValueOrError<ITickerTransformer> { Error = error };
            }

            if (!executor.VariableExists("Transform"))
            {
                executor.Dispose();
                return new ValueOrError<ITickerTransformer> { Error = new ScriptException(script, "\"Transform\" function not defined.") };
            }

            return new ValueOrError<ITickerTransformer> { Value = new TickerTransformer(executor) };
        }
    }
}
