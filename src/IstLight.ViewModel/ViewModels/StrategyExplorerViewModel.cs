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

using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace IstLight.ViewModels
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
