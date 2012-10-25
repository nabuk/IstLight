using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Simulation
{
    public class SimulationProgressEventArgs : EventArgs
    {
        public SimulationProgressEventArgs(int current, int max)
        {
            this.Current = current;
            this.Max = max;
        }

        public int Current { get; private set; }
        public int Max { get; private set; }
    }
}
