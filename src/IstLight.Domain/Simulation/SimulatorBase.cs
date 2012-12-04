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

using IstLight.Settings;
using IstLight.Strategy;
using IstLight.Synchronization;

namespace IstLight.Simulation
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
