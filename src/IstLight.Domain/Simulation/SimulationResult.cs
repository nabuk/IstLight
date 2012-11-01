using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Settings;
using IstLight.Synchronization;

namespace IstLight.Simulation
{
    public class SimulationResult : MultiQuoteList<SimulationResultQuote>
    {
        public SimulationResult(
            IReadOnlyList<SimulationResultQuote> quotes,
            SyncTickers syncTickers
            ) : base(quotes, syncTickers.Descriptions)
        {
            this.SyncTickers = syncTickers;
        }

        public SyncTickers SyncTickers { get; private set; }
        
        public ISimulationSettings Settings { get; set; }
    }
}
