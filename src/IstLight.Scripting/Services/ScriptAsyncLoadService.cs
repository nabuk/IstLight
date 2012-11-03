using System.Linq;

namespace IstLight.Services
{
    public abstract class ScriptAsyncLoadService<T> : IAsyncLoadService<T>
        where T : INamedItem
    {
        private readonly IScriptLoadService scripts;

        protected abstract ValueOrError<T> CreateInstance(Script script);

        public ScriptAsyncLoadService(IScriptLoadService scripts)
        {
            this.scripts = scripts;
        }

        public IReadOnlyList<IAsyncResult<T>> Load()
        {
            return scripts.Load().Select(script => new AsyncResultFromSyncJob<T>(() => CreateInstance(script))).AsReadOnlyList();
        }
    }
}
