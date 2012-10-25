using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IstLight.Domain.Settings;
using IstLight.Domain.Strategy;
using IstLight.Domain.Synchronization;

namespace IstLight.Domain.Simulation
{
    public abstract class SimulatorBase
    {
        protected SimulationAlgorithmRunResult SimulationAlgorithmFlow(ISimulationContext ctx, SyncTickers tickers, StrategyBase strategy, ISimulationSettings settings)
        {
            int count = tickers.Count;

            if (!ctx.Initialize(tickers, strategy, settings))
                return new SimulationAlgorithmRunResult { Error = ctx.GetLastError() };

            for (int index = 0; index < count; index++)
            {
                bool firstRun = index == 0;
                bool lastRun = index == count - 1;

                ctx.BeginStep();
                OnProgressChanged(index, count);
                if (!firstRun) ctx.ApplyInterestRate();
                if (!lastRun) ctx.ProcessPendingTransactions();
                if (!ctx.RunStrategy()) return new SimulationAlgorithmRunResult { Error = ctx.GetLastError() };
                ctx.EndStep();
            }

            OnProgressChanged(count, count);
            return new SimulationAlgorithmRunResult { Value = ctx.GetResult() };
        }

        protected virtual void OnProgressChanged(int current, int max) { }

        protected struct SimulationAlgorithmRunResult
        {
            public SimulationResult Value { get; set; }
            public string Error { get; set; }
        }
    }
}
