using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace IstLight.Simulation
{
    public class SimulationDependencies
    {
        private void BindChangeNotifications()
        {
            (TickerExplorer.Tickers as INotifyCollectionChanged).CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                    foreach (var t in e.NewItems.Cast<TickerFileViewModel>())
                        t.ExecuteWhenLoadCompletes(x => DependenciesChanged());
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                    foreach (var t in e.OldItems.Cast<TickerFileViewModel>().Where(t => t.LoadState == AsyncState.Completed))
                        DependenciesChanged();
            };

            SimulationSettings.PropertyChanged += delegate { DependenciesChanged(); };
        }

        public SimulationDependencies(
            TickerExplorerViewModel tickerExplorer,
            StrategyExplorerViewModel strategyExplorer,
            SimulationSettingsViewModel simulationSettings)
        {
            this.TickerExplorer = tickerExplorer;
            this.StrategyExplorer = strategyExplorer;
            this.SimulationSettings = simulationSettings;

            BindChangeNotifications();
        }

        public event Action DependenciesChanged = delegate { };

        public TickerExplorerViewModel TickerExplorer { get; private set; }
        public StrategyExplorerViewModel StrategyExplorer { get; private set; }
        public SimulationSettingsViewModel SimulationSettings { get; private set; }
    }
}
