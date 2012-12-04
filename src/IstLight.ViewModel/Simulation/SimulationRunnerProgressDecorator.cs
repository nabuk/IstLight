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

using IstLight.ViewModels;

namespace IstLight.Simulation
{
    public class SimulationRunnerProgressDecorator : SimulationRunnerDecoratorBase
    {
        private readonly IWindow mainWindow;
        private readonly SimulationProgressViewModel statusViewModel;
        private IWindow _statusWindow;
        private IWindow StatusWindow
        {
            get { return _statusWindow ?? (_statusWindow = mainWindow.CreateChild(statusViewModel)); }
        }

        private void ShowProgress()
        {
            statusViewModel.Status = new SimulationProgressStatus("Initializing", 0, 100);
            StatusWindow.Show();

        }
        private void HideProgress()
        {
            StatusWindow.Close();
            _statusWindow = null;
        }

        private void ProgressStatusChanged(SimulationProgressStatus status)
        {
            statusViewModel.Status = status;
        }

        public SimulationRunnerProgressDecorator(ISimulationRunner simulationRunner, IWindow mainWindow)
            : base(simulationRunner)
        {
            this.mainWindow = mainWindow;
            this.statusViewModel = new SimulationProgressViewModel(new DelegateCommand(this.Cancel));

            simulationRunner.IsRunningChanged += isRunning =>
            {
                if (isRunning) ShowProgress();
                else           HideProgress();
            };
            simulationRunner.ProgressStatus += ProgressStatusChanged;
        }
    }
}
