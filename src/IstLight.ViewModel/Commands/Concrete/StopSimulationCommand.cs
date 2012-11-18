using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Simulation;

namespace IstLight.Commands.Concrete
{
    public class StopSimulationCommand : GlobalCommandBase
    {
        public StopSimulationCommand(SimulationRunner simulationRunner)
            : base("StopSimulation", new DelegateCommand(simulationRunner.Cancel, () => simulationRunner.IsRunning))
        {
            simulationRunner.IsRunningChanged += delegate { RaiseCanExecuteChanged(); };
        }
    }
}
