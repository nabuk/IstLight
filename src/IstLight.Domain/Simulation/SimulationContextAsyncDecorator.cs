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
using IstLight.Settings;
using IstLight.Strategy;
using IstLight.Synchronization;

namespace IstLight.Simulation
{
    public class SimulationContextAsyncDecorator : ISimulationContext
    {
        private readonly ISimulationContext simulationAlgorithm;
        private readonly AbortableWorker runStrategyWorker;
        
        private readonly object cancelSync = new object();

        private bool beforeStartCancelRequest = false;

        public SimulationContextAsyncDecorator(ISimulationContext simulationAlgorithm)
        {
            if (simulationAlgorithm == null) throw new ArgumentNullException("simulationAlgorithm");

            this.simulationAlgorithm = simulationAlgorithm;
            this.runStrategyWorker = new AbortableWorker(simulationAlgorithm.RunStrategy);
        }

        public void Cancel()
        {
            lock (cancelSync)
                if (runStrategyWorker.ThreadInstance.ThreadState == ThreadState.Unstarted)
                {
                    beforeStartCancelRequest = true;
                    return;
                }

            runStrategyWorker.Abort(10);
        }

        #region IAlgorithmMethods

        bool ISimulationContext.Initialize(SyncTickers tickers, StrategyBase strategy, ISimulationSettings settings)
        {
            lock (cancelSync)
                if (!beforeStartCancelRequest)
                    runStrategyWorker.Start();

            return simulationAlgorithm.Initialize(tickers, strategy, settings) && !beforeStartCancelRequest;
        }

        bool ISimulationContext.RunStrategy()
        {
            runStrategyWorker.ResumeWork();

            lock (cancelSync)
                if (runStrategyWorker.Aborted)
                    return false;
            
            runStrategyWorker.WaitForWorkDone(Timeout.Infinite);

            return !runStrategyWorker.Aborted && runStrategyWorker.LastRunResult;
        }

        void IDisposable.Dispose()
        {
            runStrategyWorker.Dispose();
            simulationAlgorithm.Dispose();
        }

        #region Not changed

        void ISimulationContext.BeginStep()
        {
            simulationAlgorithm.BeginStep();
        }

        void ISimulationContext.EndStep()
        {
            simulationAlgorithm.EndStep();
        }

        void ISimulationContext.ProcessPendingTransactions()
        {
            simulationAlgorithm.ProcessPendingTransactions();
        }

        void ISimulationContext.ApplyInterestRate()
        {
            simulationAlgorithm.ApplyInterestRate();
        }

        SimulationResult ISimulationContext.GetResult()
        {
            return simulationAlgorithm.GetResult();
        }

        string ISimulationContext.GetLastError()
        {
            return simulationAlgorithm.GetLastError();
        }

        #endregion //Not changed

        #endregion // IAlgorithmMethods
    }
}
