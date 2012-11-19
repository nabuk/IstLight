
namespace IstLight.Simulation
{
    public class SimulationProgressStatus
    {
        public SimulationProgressStatus(string status, int current, int max)
        {
            this.Status = status;
            this.Current = current;
            this.Max = max;
        }

        public string Status { get; private set; }
        public int Current { get; private set; }
        public int Max { get; private set; }
    }
}
