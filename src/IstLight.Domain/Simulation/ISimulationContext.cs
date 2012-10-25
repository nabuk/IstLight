using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Settings;
using IstLight.Domain.Strategy;
using IstLight.Domain.Synchronization;

namespace IstLight.Domain.Simulation
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
