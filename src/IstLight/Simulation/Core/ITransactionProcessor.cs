using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Simulation.Core
{
    public interface ITransactionProcessor
    {
        bool AddRequest(int tickerIndex, double quantity);
    }
}
