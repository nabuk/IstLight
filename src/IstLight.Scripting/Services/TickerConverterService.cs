﻿using System;

namespace IstLight.Services
{
    public class TickerConverterService : ScriptAsyncLoadService<ITickerConverter>
    {
        public TickerConverterService(IScriptLoadService scripts) : base(scripts) { }

        protected override ValueOrError<ITickerConverter> CreateInstance(Script script)
        {
            Exception error = null;
            var executor = new ParallelScriptExecutor(script, out error);
            if (error != null)
            {
                executor.Dispose();
                return new ValueOrError<ITickerConverter> { Error = error };
            }

            if (!executor.VariableExists("Read"))
            {
                executor.Dispose();
                return new ValueOrError<ITickerConverter> { Error = new ScriptException(script, "\"Read\" function not defined.") };
            }

            if (!executor.VariableExists("Save"))
            {
                executor.Dispose();
                return new ValueOrError<ITickerConverter> { Error = new ScriptException(script, "\"Save\" function not defined.") };
            }

            var formatOrError = executor.SafeExecute<string>(engine => engine.GetVariable("Format"));
            if (formatOrError.IsError)
            {
                executor.Dispose();
                return new ValueOrError<ITickerConverter> { Error = new ScriptException(script, "\"Format\" variable is not defined or has wrong type.") };
            }

            return new ValueOrError<ITickerConverter> { Value = new TickerConverter(formatOrError.Value, executor) };
        }
    }
}