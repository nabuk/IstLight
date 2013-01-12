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
using System.Linq;
using IstLight.Synchronization;

namespace IstLight.Simulation
{
    public class SimulationResultQuote : IDate
    {
        private IReadOnlyList<Transaction> transactions;

        public SimulationResultQuote(
            IReadOnlyList<double> tickerQuantity,
            double cash,
            Observation observation)
        {
            this.TickerQuantity = tickerQuantity;
            this.Cash = cash;
            this.Observation = observation;
        }

        public IReadOnlyList<double> TickerQuantity { get; private set; }
        public double Cash { get; private set; }
        public Observation Observation { get; private set; }
        public IReadOnlyList<Transaction> Transactions
        {
            get
            {
                return transactions ?? new Transaction[] { }.AsReadOnlyList();
            }
            set
            {
                transactions = value;
            }
        }
        public double Interest { get; set; }
        public DateTime Date
        {
            get { return Observation.Date; }
        }
        public string Output { get; set; }
    }

    public static class SimulationResultQuoteExtensions
    {
        public static double Equity(this SimulationResultQuote quote, SyncTickers syncTickers)
        {
            return quote.Cash + Enumerable.Range(0, syncTickers.TickerCount).Select(iT => quote.TickerEquity(iT, syncTickers)).Sum();
        }

        public static double TickerEquity(this SimulationResultQuote quote, int tickerIndex, SyncTickers syncTickers)
        {
            return quote.TickerQuantity[tickerIndex] == 0 ?
                0
                :
                quote.TickerQuantity[tickerIndex]
                    * syncTickers.SimplifiedTickers[tickerIndex][quote.Observation.CurrentQuoteCount[tickerIndex] - 1].Value;
        }

        public static double ExpositionToTicker(this SimulationResultQuote quote, int tickerIndex, SyncTickers syncTickers)
        {
            return quote.TickerEquity(tickerIndex, syncTickers) / quote.Equity(syncTickers);
        }
    }
}
