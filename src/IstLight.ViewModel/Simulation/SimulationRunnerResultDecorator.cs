using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.ViewModels;

namespace IstLight.Simulation
{
    public class SimulationRunnerResultDecorator : SimulationRunnerDecoratorBase
    {
        private readonly IWindow mainWindow;
        private readonly SimulationResultViewModelFactory vmFactory;

        private void ShowResult(SimulationEndEventArgs args)
        {
            if (args.EndReason != SimulationEndReason.Completion)
                return;

            mainWindow.CreateChild(
                vmFactory.Create(args.Result)
                ).Show();
        }

        public SimulationRunnerResultDecorator(
            ISimulationRunner simulationRunner,
            IWindow mainWindow,
            SimulationResultViewModelFactory vmFactory)
            :base(simulationRunner)
        {
            this.mainWindow = mainWindow;
            this.vmFactory = vmFactory;
            this.simulationRunner.SimulationEnded += ShowResult;
        }
    }
}
