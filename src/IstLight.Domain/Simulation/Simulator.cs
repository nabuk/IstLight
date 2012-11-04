using System;
using System.Threading;
using System.Threading.Tasks;
using IstLight.Settings;
using IstLight.Strategy;
using IstLight.Synchronization;


namespace IstLight.Simulation
{
    public class Simulator : SimulatorBase
    {
        private Task simulationTask = new Task(() => { });
        private readonly object cancelRaceConditionSync = new object();
        private Action cancelSimulation = delegate { };

        protected override void OnProgressChanged(int current, int max)
        {
            ProgressChanged(this, new SimulationProgressEventArgs(current, max));
        }

        public void RunAsync(SyncTickers syncTickers, StrategyBase strategy, ISimulationSettings settings)
        {
            if (syncTickers == null) throw new ArgumentNullException("syncTickers");
            if (strategy == null) throw new ArgumentNullException("strategy");
            if (settings == null) throw new ArgumentNullException("settings");

            if (IsBusy) throw new InvalidOperationException("Already running.");

            var asyncSimulationAlgorithm = new SimulationContextAsyncDecorator(SimulationContextFactory.CreateContext());
            cancelSimulation = () => asyncSimulationAlgorithm.Cancel();

            simulationTask = Task.Factory.StartNew(() =>
            {
                using (asyncSimulationAlgorithm)
                {
                    var runResult = base.SimulationAlgorithmFlow(asyncSimulationAlgorithm, syncTickers, strategy, settings);
                    lock (cancelRaceConditionSync) { cancelSimulation = delegate { }; }
                    EndInfo = new SimulationEndEventArgs
                    {
                        Error = runResult.Error,
                        Result = runResult.Value,
                        EndReason =
                            runResult.Error != null ? SimulationEndReason.Error :
                            runResult.Value != null ? SimulationEndReason.Completion :
                            SimulationEndReason.Cancellation
                    };
                    SimulationEnded(this, EndInfo);
                }
            });

            //while (simulationTask.Status < TaskStatus.Running) ;
        }
        
        public void Cancel(bool waitForSimulationEnd = true)
        {
            lock (cancelRaceConditionSync) { cancelSimulation(); }
            if (waitForSimulationEnd) Wait(Timeout.Infinite);
        }

        public bool Wait(int millisecondsTimeout = Timeout.Infinite)
        {
            if (!IsBusy)
                return true;

            return simulationTask.Wait(millisecondsTimeout);
        }

        public bool IsBusy
        {
            get
            {
                return
                    simulationTask.Status == TaskStatus.Running
                    ||
                    simulationTask.Status == TaskStatus.WaitingForActivation
                    ||
                    simulationTask.Status == TaskStatus.WaitingForChildrenToComplete
                    ||
                    simulationTask.Status == TaskStatus.WaitingToRun;
            }
        }

        public SimulationEndEventArgs EndInfo { get; private set; }

        public event EventHandler<SimulationProgressEventArgs> ProgressChanged = delegate { };
        public event EventHandler<SimulationEndEventArgs> SimulationEnded = delegate { };
    }
}
