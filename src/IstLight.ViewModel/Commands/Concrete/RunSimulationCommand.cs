using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Simulation;

namespace IstLight.Commands.Concrete
{
    public class RunSimulationCommand : GlobalCommandBase
    {
        public RunSimulationCommand(ISimulationRunner simulationRunner)
            : base("RunSimulation", new DelegateCommand(simulationRunner.Run, () => !simulationRunner.IsRunning))
        {
            simulationRunner.IsRunningChanged += delegate { RaiseCanExecuteChanged(); };
        }
    }
}
