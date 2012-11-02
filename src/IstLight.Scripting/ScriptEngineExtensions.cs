using System;
using ScriptingWrapper;

namespace IstLight
{
    public static class ScriptEngineExtensions
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
