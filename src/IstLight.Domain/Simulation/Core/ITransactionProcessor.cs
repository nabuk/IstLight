
namespace IstLight.Simulation.Core
{
    public interface ITransactionProcessor
    {
        bool AddRequest(int tickerIndex, double quantity);
    }
}
