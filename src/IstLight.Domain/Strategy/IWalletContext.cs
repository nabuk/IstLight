
namespace IstLight.Strategy
{
    public interface IWalletContext
    {
        double Cash { get; }
        double GetQuantity(int tickerIndex);
        bool SetQuantity(int tickerIndex, double quantity);
    }
}
