using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using IstLight.Commands;

namespace IstLight.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(
            TickerExplorerViewModel tickerExplorer,
            StrategyExplorerViewModel strategyExplorer,
            ErrorListViewModel errorList,
            SimulationSettingsViewModel simulationSettings,
            GlobalCommandContainer commands)
        {
            this.TickerExplorer = tickerExplorer;
            this.ErrorList = errorList;
            this.SimulationSettings = simulationSettings;
            this.StrategyExplorer = strategyExplorer;
            this.Commands = commands;
        }

        public TickerExplorerViewModel TickerExplorer { get; private set; }
        public StrategyExplorerViewModel StrategyExplorer { get; private set; }
        public SimulationSettingsViewModel SimulationSettings { get; private set; }
        public ErrorListViewModel ErrorList { get; private set; }

        public GlobalCommandContainer Commands { get; private set; }
    }
}
