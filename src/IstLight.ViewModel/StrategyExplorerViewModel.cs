using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;

namespace IstLight
{
    public class StrategyExplorerViewModel : ViewModelBase
    {
        private readonly ObservableCollection<StrategyViewModel> strategies;
        private StrategyViewModel selectedStrategy;
        //private ICommand runCommand = DelegateCommand.NotRunnable;
        
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

        //public ICommand RunCommand
        //{
        //    get { return runCommand; }
        //    set
        //    {
        //        runCommand = value ?? DelegateCommand.NotRunnable;
        //        base.RaisePropertyChanged<ICommand>(() => RunCommand);
        //    }
        //}
    }
}
