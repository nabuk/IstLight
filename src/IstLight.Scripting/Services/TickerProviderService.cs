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
