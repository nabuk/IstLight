using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Services;

namespace IstLight.Scripting.Repositories
{
    public class BaseScriptRepositoryItem : IRepositoryItem
    {
        protected readonly ParallelScriptExecutor executor;
        public BaseScriptRepositoryItem(ParallelScriptExecutor executor)
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
