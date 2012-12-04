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
