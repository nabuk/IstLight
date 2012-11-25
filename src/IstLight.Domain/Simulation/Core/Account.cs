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