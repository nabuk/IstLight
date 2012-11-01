using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Settings;
using IstLight.Strategy;
using IstLight.Synchronization;

namespace IstLight.Simulation
{
    public interface ISimulationContext : IDisposable
    {
        bool Initialize(SyncTickers tickers, StrategyBase strategy, ISimulationSettings settings);

        void BeginStep();

        void EndStep();

        bool RunStrategy();

        void ProcessPendingTransactions();

        void ApplyInterestRate();

        SimulationResult GetResult();

        string GetLastError();
    }
}
