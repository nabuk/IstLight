using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain;
using IstLight.Domain.Services;
using ScriptingWrapper;

namespace IstLight.Scripting
{
    public class ParallelScriptExecutor
    {
        private readonly EnginePool pool;

        public ParallelScriptExecutor(Script script, out Exception error)
        {
            this.pool = new EnginePool(script);
            this.Script = script;

            if ((error = pool.AddEngineToPool().Error) != null)
                return;
        }

        public Script Script { get; private set; }

        public bool VariableExists(string variableName)
        {
            return pool.Execute(engine => engine.ContainsVariable(variableName));
        }

        public ResultOrError<T> SafeExecute<T>(Func<ScriptEngineBase, dynamic> job)
        {
            return pool.SafeExecute<T>(job);
        }
        public IAsyncResult<T> SafeExecuteAsync<T>(Func<ScriptEngineBase, dynamic> job)
        {
            return new AsyncResultFromSyncJob<T>(() => this.SafeExecute<T>(job));
        }

        public void Dispose()
        {
            pool.Dispose();
        }
    }
}
