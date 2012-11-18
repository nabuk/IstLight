using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Services;

namespace IstLight.Simulation
{
    public class SimulationRunnerErrorDecorator : SimulationRunnerDecoratorBase
    {
        private readonly IErrorReporter errorReporter;

        public SimulationRunnerErrorDecorator(ISimulationRunner simulationRunner, IErrorReporter errorReporter) : base(simulationRunner)
        {
            this.errorReporter = errorReporter;
            this.SimulationEnded += x => errorReporter.AddIfNotNull(x.Error);
        }
    }
}
