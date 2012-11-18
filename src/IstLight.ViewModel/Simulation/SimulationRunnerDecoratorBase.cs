using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Simulation
{
    public abstract class SimulationRunnerDecoratorBase : ISimulationRunner
    {
        protected readonly ISimulationRunner simulationRunner;

        public SimulationRunnerDecoratorBase(ISimulationRunner simulationRunner)
        {
            this.simulationRunner = simulationRunner;
        }

        #region ISimulationRunner
        public virtual void Run()
        {
            simulationRunner.Run();
        }

        public virtual void Cancel()
        {
            simulationRunner.Cancel();
        }

        public virtual bool IsRunning
        {
            get { return simulationRunner.IsRunning; }
        }

        public virtual event Action<bool> IsRunningChanged
        {
            add { simulationRunner.IsRunningChanged += value; }
            remove { simulationRunner.IsRunningChanged -= value; }
        }

        public virtual event Action<SimulationRunner> SimulationStarted
        {
            add { simulationRunner.SimulationStarted += value; }
            remove { simulationRunner.SimulationStarted -= value; }
        }
        public virtual event Action<SimulationProgressStatus> ProgressStatus
        {
            add { simulationRunner.ProgressStatus += value; }
            remove { simulationRunner.ProgressStatus -= value; }
        }
        public virtual event Action<SimulationEndEventArgs> SimulationEnded
        {
            add { simulationRunner.SimulationEnded += value; }
            remove { simulationRunner.SimulationEnded -= value; }
        }
        #endregion //ISimulationRunner
    }
}
