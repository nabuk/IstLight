// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

using System;

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
                ((TickerSearchResult[])engine.GetVariable("Search")(hint)).AsReadOnlyList());
        }

        public IAsyncResult<Ticker> Get(string tickerName)
        {
            return executor.SafeExecuteAsync<Ticker>(engine => engine.GetVariable("Get")(tickerName));
        }
    }
}
