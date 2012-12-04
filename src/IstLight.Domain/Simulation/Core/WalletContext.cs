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

using IstLight.Strategy;

namespace IstLight.Simulation.Core
{
    public class WalletContext : IWalletContext
    {
        private readonly IQuoteContext quoteContext;
        private readonly IAccount account;
        private readonly ITransactionProcessor transactionProcessor;

        public WalletContext(
            IQuoteContext quoteContext,
            IAccount account,
            ITransactionProcessor transactionProcessor)
        {
            this.quoteContext = quoteContext;
            this.account = account;
            this.transactionProcessor = transactionProcessor;
        }

        #region IWalletContext
        public double Cash
        {
            get
            {
                return account.Cash;
            }
        }

        public double GetQuantity(int tickerIndex)
        {
            return account.GetTickerQuantity(tickerIndex);
        }

        public bool SetQuantity(int tickerIndex, double quantity)
        {
            return transactionProcessor.AddRequest(tickerIndex, quantity);
        }
        #endregion //IWalletContext
    }
}