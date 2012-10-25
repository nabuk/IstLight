using System;

namespace IstLight.Domain.Simulation.Core
{
    public interface IAccount
    {
        double Cash { get; }
        double GetTickerQuantity(int tickerIndex);
    }
}
