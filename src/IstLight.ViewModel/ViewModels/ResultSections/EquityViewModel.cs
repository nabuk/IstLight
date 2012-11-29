using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Simulation;

namespace IstLight.ViewModels.ResultSections
{
    public class EquityViewModel : ISectionViewModel
    {
        private readonly SimulationResult result;

        public EquityViewModel(SimulationResult result)
        {
            this.result = result;
            this.Rows = result.Select((x, i) => new EquityRowViewModel(result, i)).ToArray();
        }

        public IEnumerable<EquityRowViewModel> Rows { get; private set; }

        public DateTime From { get { return result.From; } }
        public DateTime To { get { return result.To; } }

        public IEnumerable<double> Points { get { return Rows.Select(x => x.Total); } }

        #region ISectionViewModel
        public string Header
        {
            get { return "Equity"; }
        }
        #endregion //ISectionViewModel
    }
}
