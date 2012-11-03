
namespace IstLight.Services
{
    public class ScriptNamedItemBase : INamedItem
    {
        protected readonly ParallelScriptExecutor executor;
        public ScriptNamedItemBase(ParallelScriptExecutor executor)
        {
            this.executor = executor;

            string name = null;
            executor.SafeExecute<bool>(engine => engine.TryGetVariable("Name", out name));
            this.Name = name ?? executor.Script.Name;
        }

        public string Name
        {
            get;
            private set;
        }

        public virtual void Dispose()
        {
            executor.Dispose();
        }
    }
}
