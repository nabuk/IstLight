using IstLight.Simulation;

namespace IstLight.Commands.Concrete
{
    public class StopSimulationCommand : GlobalCommandBase
    {
        public StopSimulationCommand(ISimulationRunner simulationRunner)
            : base("StopSimulation", new DelegateCommand(simulationRunner.Cancel, () => simulationRunner.IsRunning))
        {
            simulationRunner.IsRunningChanged += delegate { RaiseCanExecuteChanged(); };
        }
    }
}
