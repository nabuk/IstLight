using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace IstLight
{
    public class StrategyExplorerViewModel : ViewModelBase
    {
        private readonly ObservableCollection<StrategyViewModel> strategies;
        private StrategyViewModel selectedStrategy;
        
        public StrategyExplorerViewModel()
        {
            this.selectedStrategy = new StrategyViewModel();
            this.strategies = new ObservableCollection<StrategyViewModel>(new StrategyViewModel[] { selectedStrategy });
            this.Strategies = new ReadOnlyObservableCollection<StrategyViewModel>(strategies);
        }

        public ReadOnlyObservableCollection<StrategyViewModel> Strategies { get; private set; }
        public StrategyViewModel SelectedStrategy
        {
            get { return selectedStrategy; }
            set
            {
                if (value == selectedStrategy)
                    return;

                selectedStrategy = value;
                base.RaisePropertyChanged<StrategyViewModel>(() => SelectedStrategy);
            }
        }
    }
}
