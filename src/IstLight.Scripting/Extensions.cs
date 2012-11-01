using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight;
using ScriptingWrapper;

namespace IstLight.Scripting
{
    public static class Extensions
    {
        public static ValueOrError<T> SafeExecute<T>(this ScriptEngineBase scriptEngine, Func<ScriptEngineBase, dynamic> job)
        {
            T result;
            try { result = job(scriptEngine); }
            catch (Exception ex) { return new ValueOrError<T> { Error = ex }; }
            return new ValueOrError<T> { Value = result };
        }
    }
}
