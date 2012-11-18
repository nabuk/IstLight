using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using IstLight.Settings;
using IstLight.Strategy;
using IstLight.Synchronization;

namespace IstLight.Simulation
{
    public class SimulationInput
    {
        public SimulationInput(
            ISyncTickersGetter syncTickersGetter,
            IStrategyCreator strategyCreator,
            ISimulationSettingsGetter simulationSettingsGetter)
        {
            this.SyncTickersGetter = syncTickersGetter;
            this.StrategyCreator = strategyCreator;
            this.SimulationSettingsGetter = simulationSettingsGetter;
        }

        public ISyncTickersGetter SyncTickersGetter { get; private set; }
        public IStrategyCreator StrategyCreator { get; private set; }
        public ISimulationSettingsGetter SimulationSettingsGetter { get; private set; }
    }
}
