using System;

namespace IstLight.Services
{
    public class TickerProviderService : ScriptAsyncLoadService<ITickerProvider>
    {
        public TickerProviderService(IScriptLoadService scripts) : base(scripts) { }

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
