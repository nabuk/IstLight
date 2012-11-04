using System;
using System.Linq;

namespace IstLight.Services
{
    public class TickerProvider : ScriptNamedItemBase, ITickerProvider
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
                ((object[])engine.GetVariable("Search")(hint)).Cast<TickerSearchResult>().ToArray().AsReadOnlyList());
        }

        public IAsyncResult<Ticker> Get(string tickerName)
        {
            return executor.SafeExecuteAsync<Ticker>(engine => engine.GetVariable("Get")(tickerName));
        }
    }
}
