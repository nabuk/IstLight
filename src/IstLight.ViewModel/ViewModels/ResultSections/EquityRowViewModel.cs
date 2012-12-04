// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

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
            this.Interest = resultQuote.Interest;
            this.Total = resultQuote.Equity(result.SyncTickers);
            this.Portfolio = Total - Cash;
            this.Commissions = resultQuote.Transactions.Select(t => t.Commission).Sum();
            this.TransactionCount = resultQuote.Transactions.Count;
            this.TransactionTotal = resultQuote.Transactions.Select(t => t.CashFlow).Sum();

            this.Bar = barIndex + 1;
        }

        public int Bar { get; private set; }
        public DateTime Date { get; private set; }
        public double Cash { get; private set; }
        public double Interest { get; private set; }
        public double Portfolio { get; private set; }
        public double Commissions { get; private set; }
        public int TransactionCount { get; private set; }
        public double TransactionTotal { get; private set; }
        public double Total { get; private set; }
    }
}
