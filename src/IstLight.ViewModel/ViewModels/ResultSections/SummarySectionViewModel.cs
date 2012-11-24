using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using IstLight.Services;
using IstLight.Simulation;

namespace IstLight.ViewModels.ResultSections
{
    public class SummarySectionViewModel : ViewModelBase, ISectionViewModel
    {
        private readonly SimulationResult result;
        private readonly IReadOnlyList<IAsyncResult<IResultAnalyzer>> analyzers;

        public SummarySectionViewModel(IReadOnlyList<IAsyncResult<IResultAnalyzer>> analyzers, SimulationResult result)
        {
            this.analyzers = analyzers;
            this.result = result;

            this.Groups = new ReadOnlyCollection<SummaryGroupViewModel>(
                analyzers.Select(a => new SummaryGroupViewModel(a, result)).ToList());

        }

        #region ISectionViewModel
        public string Header
        {
            get { return "Summary"; }
        }
        #endregion //ISectionViewModel

        public ReadOnlyCollection<SummaryGroupViewModel> Groups { get; private set; }
    }
}