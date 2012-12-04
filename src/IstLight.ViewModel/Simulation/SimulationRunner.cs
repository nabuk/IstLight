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