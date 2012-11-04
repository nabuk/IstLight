using System;

namespace IstLight.Simulation
{
    [Flags]
    public enum TransactionType : int
    {
        Buy = 0,
        Sell = 1
    }
}
