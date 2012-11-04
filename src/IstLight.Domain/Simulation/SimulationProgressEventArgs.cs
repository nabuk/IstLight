using System;

namespace IstLight.Simulation
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
