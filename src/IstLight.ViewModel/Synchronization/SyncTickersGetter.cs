using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Services;
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
                .Select(t => t.AsyncTicker.Result)
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
