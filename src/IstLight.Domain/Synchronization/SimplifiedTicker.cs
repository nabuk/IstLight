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

using System.Collections.Generic;

namespace IstLight.Synchronization
{
    public class SimplifiedTicker : IReadOnlyList<ISimpleTickerQuote>
    {
        private readonly IReadOnlyList<ISimpleTickerQuote> quotes;

        public SimplifiedTicker(IReadOnlyList<ISimpleTickerQuote> quotes)
        {
            this.quotes = quotes ?? new ISimpleTickerQuote[0].AsReadOnlyList();
        }

        public ISimpleTickerQuote this[int index]
        {
            get { return quotes[index]; }
        }

        public int Count
        {
            get { return quotes.Count; }
        }

        public IEnumerator<ISimpleTickerQuote> GetEnumerator()
        {
            return quotes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return quotes.GetEnumerator();
        }
    }
}
