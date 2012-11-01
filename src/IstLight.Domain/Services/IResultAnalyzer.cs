using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IstLight.Simulation;

namespace IstLight.Services
{
    public interface IResultAnalyzer : IRepositoryItem
    {
        string Category { get; }
        IAsyncResult<string> Analyze(SimulationResult result);
    }
}
