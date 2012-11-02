using IstLight.Simulation;

namespace IstLight.Services
{
    public interface IResultAnalyzer : IServiceItem
    {
        string Category { get; }
        IAsyncResult<string> Analyze(SimulationResult result);
    }
}
