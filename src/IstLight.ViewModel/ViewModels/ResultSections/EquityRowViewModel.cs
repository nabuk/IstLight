using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Settings;
using IstLight.Simulation;
using IstLight.Simulation.Core;

namespace IstLight.ViewModels.ResultSections
{
    public class EquityRowViewModel
    {
        public EquityRowViewModel(SimulationResult result, int barIndex)
        {
            var resultQuote = result[barIndex];
            this.Date = resultQuote.Date;
            this.Cash = resultQuote.Cash;
            this.Total = resultQuote.Equity(result.SyncTickers);
            this.Portfolio = Total - Cash;
            this.Commissions =
                resultQuote.Transactions.Select(t =>
                    result.Settings.Get<CommissionSetting>().ComputeFee(
                        t.Quantity,
                        result.SyncTickers.SimplifiedTickers[t.TickerIndex][
                            result.SyncTickers[barIndex].CurrentQuoteCount[t.TickerIndex] - 1].Value))
                .Sum();

            this.Bar = barIndex + 1;
        }

        public int Bar { get; private set; }
        public DateTime Date { get; private set; }
        public double Cash { get; private set; }
        public double Portfolio { get; private set; }
        public double Commissions { get; private set; }
        public double Total { get; private set; }
    }
}
