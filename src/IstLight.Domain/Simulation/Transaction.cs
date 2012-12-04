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
using IstLight.Synchronization;
namespace IstLight.Simulation
{
    public class Transaction
    {
        public Transaction(TransactionType type, int tickerIndex, double quantity, double netProfitRate, double commission, double cashFlow)
        {
            this.Type = type;
            this.TickerIndex = tickerIndex;
            this.Quantity = quantity;
            this.NetProfitRate = netProfitRate;
            this.Commission = commission;
            this.CashFlow = cashFlow;
        }

        /// <summary>
        /// Transaction type description flags.
        /// </summary>
        public TransactionType Type { get; private set; }

        /// <summary>
        /// Ticker index associated with this transaction.
        /// </summary>
        public int TickerIndex { get; private set; }

        public double Quantity { get; private set; }

        /// <summary>
        /// Transaction level profit (sell only).
        /// </summary>
        public double NetProfitRate { get; private set; }

        public double Commission { get; private set; }

        public double CashFlow { get; private set; }

        public double NetProfit
        {
            get
            {
                if (NetProfitRate == 0) return 0;
                return (CashFlow / (1 + NetProfitRate)) * NetProfitRate;
            }
        }
    }
}
