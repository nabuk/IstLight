using IstLight.ViewModels;

namespace IstLight.Settings
{
    public class SimulationSettingsGetter : ISimulationSettingsGetter
    {
        private readonly SimulationSettingsViewModel simulationSettings;

        public SimulationSettingsGetter(SimulationSettingsViewModel simulationSettings)
        {
            this.simulationSettings = simulationSettings;
        }

        public ISimulationSettings Get()
        {
            return simulationSettings.GetSimulationSettings();
        }
    }
}
