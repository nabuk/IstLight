using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Services.Decorators
{
    public class ResultAnalyzerErrorDecorator : NamedItemBaseErrorDecorator<IResultAnalyzer>, IResultAnalyzer
    {
        public ResultAnalyzerErrorDecorator(IResultAnalyzer itemToDecorate, IErrorReporter errorReporter)
            :base(itemToDecorate, errorReporter) { }

        #region IResultAnalyzer
        public string Category
        {
            get { return base.itemToDecorate.Category; }
        }

        public IAsyncResult<IReadOnlyList<KeyValuePair<string, string>>> Analyze(Simulation.SimulationResult result)
        {
            return Decorate(itemToDecorate.Analyze(result));
        }
        #endregion //IResultAnalyzer
    }
}
