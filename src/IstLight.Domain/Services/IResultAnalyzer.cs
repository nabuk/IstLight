using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IstLight.Domain.Simulation;

namespace IstLight.Domain.Services
{
    public interface IResultAnalyzer : IRepositoryItem
    {
        string Category { get; }
        IAsyncResult<string> Analyze(SimulationResult result);
    }
}
