using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Settings;
using IstLight.Domain.Synchronization;

namespace IstLight.Domain.Simulation
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
