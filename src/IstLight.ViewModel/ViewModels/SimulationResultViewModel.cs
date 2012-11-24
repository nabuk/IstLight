using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.ViewModels
{
    public class SimulationResultViewModel
    {
        public SimulationResultViewModel(IEnumerable<ISectionViewModel> sections)
        {
            this.Sections = sections;
        }

        public IEnumerable<ISectionViewModel> Sections { get; private set; }
    }
}
