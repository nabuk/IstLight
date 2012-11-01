using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Simulation.Core
{
    public class TransactionRequest
    {
        public TransactionRequest(int tickerIndex, double quantity, int delay)
        {
            this.TickerIndex = tickerIndex;
            this.NewQuantity = quantity;
            this.Delay = delay;
        }

        public int TickerIndex { get; private set; }
        public double NewQuantity { get; private set; }
        public int Delay { get; private set; }

        public void DecrementDelay() { Delay--; }
    }
}
