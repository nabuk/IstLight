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
using IstLight.Services;
using ScriptingWrapper;

namespace IstLight
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

        public ValueOrError<T> SafeExecute<T>(Func<ScriptEngineBase, dynamic> job)
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
