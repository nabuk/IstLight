using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight;
using IstLight.Extensions;
using IstLight.Services;
using ScriptingWrapper;

namespace IstLight.Scripting.Repositories
{
    public abstract class ScriptRepositoryBase<T> : IBaseRepository<T>
        where T : IRepositoryItem
    {
        private readonly IScriptRepository scripts;

        protected abstract ResultOrError<T> CreateInstance(Script script);

        public ScriptRepositoryBase(IScriptRepository scripts)
        {
            this.scripts = scripts;
        }

        public IReadOnlyList<IAsyncResult<T>> Load()
        {
            return scripts.Load().Select(script => new AsyncResultFromSyncJob<T>(() => CreateInstance(script))).AsReadOnlyList();
        }
    }
}
