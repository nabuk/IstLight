// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using ScriptingWrapper;

namespace IstLight
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

        private ValueOrError<ScriptEngineBase> CreateEngine()
        {
            ScriptEngineBase engine;

            if ((engine = ScriptEngineFactory.TryCreateEngine(script.Extension)) == null)
                return new ValueOrError<ScriptEngineBase> { Error = new ArgumentException("Invalid script extension: " + script.Extension) };

            engine.AddSearchPaths(Path.GetDirectoryName(Assembly.GetAssembly(typeof(Ticker)).Location));

            if (!engine.SetScript(script.Content) || !engine.Execute())
                return new ValueOrError<ScriptEngineBase>
                    { Error = new InvalidOperationException(string.Format("{0}.{1}: {2}", script.Name, script.Extension, engine.LastError)) };

            return new ValueOrError<ScriptEngineBase> { Value = engine };
        }

        private ValueOrError<ScriptEngineBase> TakeFromPoolOrError()
        {
            ScriptEngineBase engine;
            if (!pool.TryTake(out engine))
            {
                var engineOrError = CreateEngine();
                if (engineOrError.IsError)
                    return new ValueOrError<ScriptEngineBase> { Error = engineOrError.Error };
                engine = engineOrError.Value;
            }
            return new ValueOrError<ScriptEngineBase> { Value = engine };
        }

        public ValueOrError<bool> AddEngineToPool()
        {
            var engineOrError = CreateEngine();
            if (engineOrError.IsError)
                return new ValueOrError<bool> { Error = engineOrError.Error };
            pool.Add(engineOrError.Value);
            return new ValueOrError<bool> { Value = true };
        }

        public T Execute<T>(Func<ScriptEngineBase,T> job)
        {
            var engineOrError = TakeFromPoolOrError();
            if(engineOrError.IsError)
                throw engineOrError.Error;
            return job(engineOrError.Value);
        }

        public ValueOrError<T> SafeExecute<T>(Func<ScriptEngineBase, dynamic> job)
        {
            var engineOrError = TakeFromPoolOrError();
            if (engineOrError.IsError)
                return new ValueOrError<T> { Error = engineOrError.Error };
            ScriptEngineBase engine = engineOrError.Value;

            var executeResult = engine.SafeExecute<T>(job);
            if (!executeResult.IsError)
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
