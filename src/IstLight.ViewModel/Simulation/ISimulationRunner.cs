using System;
namespace IstLight.Simulation
{
    public interface ISimulationRunner
    {
        void Run();
        void Cancel();
        bool IsRunning { get; }
        event Action<bool> IsRunningChanged;
        event Action<SimulationRunner> SimulationStarted;
        event Action<SimulationProgressStatus> ProgressStatus;
        event Action<SimulationEndEventArgs> SimulationEnded;
    }
}
