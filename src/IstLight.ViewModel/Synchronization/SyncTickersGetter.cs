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
using System.Linq;
using IstLight.Settings;
using IstLight.ViewModels;

namespace IstLight.Synchronization
{
    public class SyncTickersGetter : ISyncTickersGetter
    {
        private readonly TickerExplorerViewModel tickerExplorer;
        private readonly ISimulationSettingsGetter settingsGetter;

        public SyncTickersGetter(TickerExplorerViewModel tickerExplorer, ISimulationSettingsGetter settingsGetter)
        {
            this.tickerExplorer = tickerExplorer;
            this.settingsGetter = settingsGetter;
        }

        public ValueOrError<Func<SyncTickers>> TryGet()
        {
            var tickers =
                tickerExplorer.Tickers
                .Where(t => t.LoadState == AsyncState.Completed)
                .Select(t => t.Ticker)
                .AsReadOnlyList();
            var settings = settingsGetter.Get();

            string error;
            if (!SyncTickersFactory.CanSynchronize(tickers, settings, out error))
                return new ValueOrError<Func<SyncTickers>> { Error = new Exception(error) };
            else
                return new ValueOrError<Func<SyncTickers>> { Value = () => SyncTickersFactory.Synchronize(tickers, settings) };
        }
    }
}
