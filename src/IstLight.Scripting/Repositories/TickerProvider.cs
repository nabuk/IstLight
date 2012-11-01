using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight;
using IstLight.Services;
using IstLight.Extensions;

namespace IstLight.Scripting.Repositories
{
    public class TickerProvider : BaseScriptRepositoryItem, ITickerProvider
    {
        public TickerProvider(ParallelScriptExecutor executor) : base(executor)
        {
            this.CanSearch = executor.VariableExists("Search");
        }

        public bool CanSearch
        {
            get;
            private set;
        }
        public IAsyncResult<IReadOnlyList<TickerSearchResult>> Search(string hint)
        {
            if (!CanSearch) throw new InvalidOperationException("Cannot search");
            return executor.SafeExecuteAsync<IReadOnlyList<TickerSearchResult>>(engine =>
                (engine.GetVariable("Search")(hint) as TickerSearchResult[]).AsReadOnlyList());
        }
        public IAsyncResult<Ticker> Get(string tickerName)
        {
            return executor.SafeExecuteAsync<Ticker>(engine => engine.GetVariable("Get")(tickerName));
        }
    }
}
