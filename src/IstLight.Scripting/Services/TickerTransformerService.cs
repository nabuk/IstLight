using System;


namespace IstLight.Services
{
    public class TickerTransformerService : ScriptAsyncLoadService<ITickerTransformer>
    {
        public TickerTransformerService(IScriptLoadService scripts) : base(scripts) { }

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
