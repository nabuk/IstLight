using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using IstLight;
using ScriptingWrapper;

namespace IstLight.Scripting
{
    public class EnginePool : IDisposable
    {
        private readonly Script script;
        private readonly ConcurrentBag<ScriptEngineBase> pool = new ConcurrentBag<ScriptEngineBase>();

        public EnginePool(Script script)
        {
            if (script == null) throw new ArgumentNullException("script");
            this.script = script;
        }

        private ResultOrError<ScriptEngineBase> CreateEngine()
        {
            ScriptEngineBase engine;

            if ((engine = ScriptEngineFactory.TryCreateEngine(script.Extension)) == null)
                return new ResultOrError<ScriptEngineBase> { Error = new ArgumentException("Invalid script extension: " + script.Extension) };

            engine.AddSearchPaths(Path.GetDirectoryName(Assembly.GetAssembly(typeof(Ticker)).Location));

            if (!engine.SetScript(script.Content) || !engine.Execute())
                return new ResultOrError<ScriptEngineBase>
                    { Error = new InvalidOperationException(string.Format("{0}.{1}: {2}", script.Name, script.Extension, engine.LastError)) };

            return new ResultOrError<ScriptEngineBase> { Result = engine };
        }

        private ResultOrError<ScriptEngineBase> TakeFromPoolOrError()
        {
            ScriptEngineBase engine;
            if (!pool.TryTake(out engine))
            {
                var engineOrError = CreateEngine();
                if (engineOrError.Error != null)
                    return new ResultOrError<ScriptEngineBase> { Error = engineOrError.Error };
                engine = engineOrError.Result;
            }
            return new ResultOrError<ScriptEngineBase> { Result = engine };
        }

        public ResultOrError<bool> AddEngineToPool()
        {
            var engineOrError = CreateEngine();
            if (engineOrError.Error != null)
                return new ResultOrError<bool> { Error = engineOrError.Error };
            pool.Add(engineOrError.Result);
            return new ResultOrError<bool> { Result = true };
        }

        public T Execute<T>(Func<ScriptEngineBase,T> job)
        {
            var engineOrError = TakeFromPoolOrError();
            if(engineOrError.Error != null)
                throw engineOrError.Error;
            return job(engineOrError.Result);
        }

        public ResultOrError<T> SafeExecute<T>(Func<ScriptEngineBase, dynamic> job)
        {
            var engineOrError = TakeFromPoolOrError();
            if (engineOrError.Error != null)
                return new ResultOrError<T> { Error = engineOrError.Error };
            ScriptEngineBase engine = engineOrError.Result;

            var executeResult = engine.SafeExecute<T>(job);
            if (executeResult.Error != null)
                pool.Add(engine);
            else
                engine.Dispose();

            return executeResult;
        }

        public void Dispose()
        {
            ScriptEngineBase engine;
            while (pool.TryTake(out engine))
                engine.Dispose();
        }
    }
}
