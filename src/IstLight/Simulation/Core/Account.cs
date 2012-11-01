using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Strategy;

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

        public void ApplyInterestRate(double annualInterestRate, TimeSpan span)
        {
            cash *= Math.Pow(annualInterestRate + 1.0, span.TotalDays / 365.25);
        }
    }
}
