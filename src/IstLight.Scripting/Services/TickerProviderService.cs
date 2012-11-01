using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight;
using IstLight.Services;

namespace IstLight.Scripting.Services
{
    public class TickerProviderService : ScriptServiceBase<ITickerProvider>, ITickerProviderService
    {
        public TickerProviderService(IScriptService scripts) : base(scripts) { }

        protected override ValueOrError<ITickerProvider> CreateInstance(Script script)
        {
            Exception error = null;
            var executor = new ParallelScriptExecutor(script, out error);
            if (error != null)
            {
                executor.Dispose();
                return new ValueOrError<ITickerProvider> { Error = error };
            }
            if (!executor.VariableExists("Get"))
            {
                executor.Dispose();
                return new ValueOrError<ITickerProvider> { Error = new ScriptException(script, "\"Get\" function not defined.") };
            }
            return new ValueOrError<ITickerProvider> { Value = new TickerProvider(executor) };
        }
    }
}
