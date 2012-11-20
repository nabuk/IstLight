using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Simulation;

namespace IstLight.ViewModels
{
    public class SimulationResultViewModelFactory
    {
        
        
        public SimulationResultViewModel Create(SimulationResult result)
        {
            return new SimulationResultViewModel();
        }
    }
}
