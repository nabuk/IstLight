using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Simulation.Core;

namespace IstLight.Domain.Simulation
{
    public static class SimulationContextFactory
    {
        public static ISimulationContext CreateContext()
        {
            return new SimulationContext();
        }
    }
}
