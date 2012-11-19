﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            this.statusViewModel = new SimulationProgressViewModel();

            simulationRunner.IsRunningChanged += isRunning =>
            {
                if (isRunning) ShowProgress();
                else           HideProgress();
            };
            simulationRunner.ProgressStatus += ProgressStatusChanged;
        }
    }
}
