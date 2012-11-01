using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Simulation;
using Xunit;

namespace IstLight.UnitTests.Simulation
{
    public class SimulationContextFactoryTests
    {
        [Fact]
        public void CreateContext_ExecutesWithoutException()
        {
            Assert.DoesNotThrow(() => SimulationContextFactory.CreateContext());
        }

        [Fact]
        public void CreateContext_ReturnsISimulationContextImplementation()
        {
            Assert.IsAssignableFrom(typeof(ISimulationContext), SimulationContextFactory.CreateContext());
        }
    }
}
