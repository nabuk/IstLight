using IstLight.Simulation.Core;

namespace IstLight.Simulation
{
    public static class SimulationContextFactory
    {
        public static ISimulationContext CreateContext()
        {
            return new SimulationContext();
        }
    }
}
