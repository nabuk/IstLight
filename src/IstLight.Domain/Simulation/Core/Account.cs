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

namespace IstLight.Simulation.Core
{
    public class Account : IAccount
    {
        private double cash;
        private readonly Dictionary<int, double> tickerQuantity = new Dictionary<int, double>();

        public Account(double cash)
        {
            this.cash = cash;
        }

        #region IAccount
        public double Cash { get { return cash; } }

        public double GetTickerQuantity(int tickerIndex)
        {
            return tickerQuantity.ContainsKey(tickerIndex) ? tickerQuantity[tickerIndex] : 0;
        }
        #endregion IAccount

        public void ChangeTickerQuantity(int tickerIndex, double quantityChange, double cashChange)
        {
            if (!tickerQuantity.ContainsKey(tickerIndex))
                tickerQuantity.Add(tickerIndex, 0);

            tickerQuantity[tickerIndex] += quantityChange;
            cash += cashChange;
        }

        //returns interest equity (not rate)
        public double ApplyInterestRate(double annualInterestRate, TimeSpan span)
        {
            double prevCash = cash;
            double interestRate = Math.Pow(annualInterestRate + 1.0, span.TotalDays / 365.25);
            
            cash *= interestRate;

            return cash - prevCash;
        }
    }
}