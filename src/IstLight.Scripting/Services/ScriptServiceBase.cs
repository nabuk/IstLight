using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptingWrapper;
using IstLight.Extensions;

namespace IstLight.Services
{
    public abstract class ScriptServiceBase<T> : IBaseService<T>
        where T : IServiceItem
    {
        private readonly IScriptService scripts;

        protected abstract ValueOrError<T> CreateInstance(Script script);

        public ScriptServiceBase(IScriptService scripts)
        {
            this.scripts = scripts;
        }

        public IReadOnlyList<IAsyncResult<T>> Load()
        {
            return scripts.Load().Select(script => new AsyncResultFromSyncJob<T>(() => CreateInstance(script))).AsReadOnlyList();
        }
    }
}
