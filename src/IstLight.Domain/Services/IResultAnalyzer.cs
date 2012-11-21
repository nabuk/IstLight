using System.Collections.Generic;
using IstLight.Simulation;

namespace IstLight.Services
{
    public interface IResultAnalyzer : INamedItem
    {
        string Category { get; }
        IAsyncResult<IReadOnlyList<KeyValuePair<string,string>>> Analyze(SimulationResult result);
    }
}
