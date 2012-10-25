using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Simulation
{
    [Flags]
    public enum TransactionType : int
    {
        Buy = 0,
        Sell = 1
    }
}
