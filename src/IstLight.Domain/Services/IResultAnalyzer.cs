using IstLight.Simulation;

namespace IstLight.Services
{
    public interface IResultAnalyzer : INamedItem
    {
        string Category { get; }
        IAsyncResult<string> Analyze(SimulationResult result);
    }
}
