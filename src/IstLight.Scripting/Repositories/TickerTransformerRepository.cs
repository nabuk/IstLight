﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain;
using IstLight.Domain.Services;


namespace IstLight.Scripting.Repositories
{
    public class TickerTransformerRepository : ScriptRepositoryBase<ITickerTransformer>, ITickerTransformerRepository
    {
        public TickerTransformerRepository(IScriptRepository scripts) : base(scripts) { }

        protected override ResultOrError<ITickerTransformer> CreateInstance(Script script)
        {
            Exception error = null;
            var executor = new ParallelScriptExecutor(script, out error);
            if (error != null)
            {
                executor.Dispose();
                return new ResultOrError<ITickerTransformer> { Error = error };
            }

            if (!executor.VariableExists("Transform"))
            {
                executor.Dispose();
                return new ResultOrError<ITickerTransformer> { Error = new ScriptException(script, "\"Transform\" function not defined.") };
            }

            return new ResultOrError<ITickerTransformer> { Result = new TickerTransformer(executor) };
        }
    }
}
