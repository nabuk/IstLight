using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Settings;
using IstLight.Domain.Simulation;

namespace IstLight.Domain.Report
{
    public static class SimulationResultReport
    {
        public static IEnumerable<ScalarReport> Analyze(this SimulationResult simulationResult, ISimulationSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
