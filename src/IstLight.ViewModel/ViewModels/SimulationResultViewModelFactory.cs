using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Services;
using IstLight.Simulation;
using IstLight.ViewModels.ResultSections;

namespace IstLight.ViewModels
{
    public class SimulationResultViewModelFactory
    {
        private readonly IAsyncLoadService<IResultAnalyzer> loadAnalyzersService;

        public SimulationResultViewModelFactory(IAsyncLoadService<IResultAnalyzer> loadAnalyzersService)
        {
            this.loadAnalyzersService = loadAnalyzersService;
        }
        
        public SimulationResultViewModel Create(SimulationResult result)
        {
            var analyzerList = loadAnalyzersService.Load();

            return new SimulationResultViewModel(
                new ISectionViewModel[]
                {
                    new SummarySectionViewModel(analyzerList,result),
                    new EquityViewModel(result)
                });
        }
    }
}
