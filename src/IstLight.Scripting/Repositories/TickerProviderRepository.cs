using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain;
using IstLight.Domain.Services;

namespace IstLight.Scripting.Repositories
{
    public class TickerProviderRepository : ScriptRepositoryBase<ITickerProvider>, ITickerProviderRepository
    {
        public TickerProviderRepository(IScriptRepository scripts) : base(scripts) { }

        protected override ResultOrError<ITickerProvider> CreateInstance(Script script)
        {
            Exception error = null;
            var executor = new ParallelScriptExecutor(script, out error);
            if (error != null)
            {
                executor.Dispose();
                return new ResultOrError<ITickerProvider> { Error = error };
            }
            if (!executor.VariableExists("Get"))
            {
                executor.Dispose();
                return new ResultOrError<ITickerProvider> { Error = new ScriptException(script, "\"Get\" function not defined.") };
            }
            return new ResultOrError<ITickerProvider> { Result = new TickerProvider(executor) };
        }
    }
}
