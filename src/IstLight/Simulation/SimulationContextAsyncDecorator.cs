using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
