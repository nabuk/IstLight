using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace IstLight.Simulation
{
    public class SimulationRunner : IstLight.Simulation.ISimulationRunner
    {
        private readonly SimulationInput input;
        private readonly Simulator simulator;
        private bool isRunning;
        private readonly object isRunningLocker = new object();
        private Dispatcher dispatcher;
        private DateTime lastUpdate;

        private void StartSimulation()
        {
            lock (isRunningLocker)
            {
                if (isRunning) return;
                isRunning = true;
            }

            dispatcher.InvokeIfRequired(() =>
            {
                IsRunningChanged(isRunning);
                SimulationStarted(this);
            });

            var strategy = input.StrategyCreator.CreateStrategy();
            var settings = input.SimulationSettingsGetter.Get();
            var syncTickersGetter = input.SyncTickersGetter.TryGet();
            if (syncTickersGetter.IsError)
            {
                EndSimulation(new SimulationEndEventArgs { EndReason = SimulationEndReason.Error, Error = syncTickersGetter.Error.Message });
                return;
            }
            lastUpdate = DateTime.MinValue;
            simulator.RunAsync(syncTickersGetter.Value(), strategy, settings);
        }
        private void ReportProgress(SimulationProgressStatus progress)
        {
            if ((DateTime.Now - lastUpdate) > TimeSpan.FromMilliseconds(10))
            {
                lastUpdate = DateTime.Now;
                dispatcher.InvokeIfRequired(() => ProgressStatus(progress));
            }
        }
        private void EndSimulation(SimulationEndEventArgs args)
        {
            lock (isRunningLocker)
                isRunning = false;

            dispatcher.InvokeIfRequired(() =>
            {
                SimulationEnded(args);
                IsRunningChanged(isRunning);
            });
        }

        public SimulationRunner(SimulationInput input)
        {
            this.input = input;
            this.simulator = new Simulator();
            this.simulator.ProgressChanged += (s, e) => ReportProgress(new SimulationProgressStatus("Simulating", e.Current, e.Max));
            this.simulator.SimulationEnded += (s, e) => EndSimulation(e);
            this.dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void Run()
        {
            Task.Factory.StartNew(StartSimulation);
        }
        public void Cancel()
        {
            if (!isRunning)
                return;

            simulator.Cancel(false);
        }
        public bool IsRunning { get { return isRunning; } }
        public event Action<bool> IsRunningChanged = delegate { };
        public event Action<SimulationRunner> SimulationStarted = delegate { };
        public event Action<SimulationProgressStatus> ProgressStatus = delegate { };
        public event Action<SimulationEndEventArgs> SimulationEnded = delegate { };
    }
}