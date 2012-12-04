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
