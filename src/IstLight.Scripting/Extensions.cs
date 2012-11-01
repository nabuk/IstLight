using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain;
using ScriptingWrapper;

namespace IstLight.Scripting
{
    public static class Extensions
    {
        public static ResultOrError<T> SafeExecute<T>(this ScriptEngineBase scriptEngine, Func<ScriptEngineBase, dynamic> job)
        {
            T result;
            try { result = job(scriptEngine); }
            catch (Exception ex) { return new ResultOrError<T> { Error = ex }; }
            return new ResultOrError<T> { Result = result };
        }
    }
}
