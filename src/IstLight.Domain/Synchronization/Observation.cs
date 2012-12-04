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

namespace IstLight.Synchronization
{
    public class Observation : IDate
    {
        public Observation(IReadOnlyList<int> currentQuoteCount, DateTime date)
        {
            if (currentQuoteCount == null) throw new ArgumentNullException("quoteCount");
            if (!currentQuoteCount.Any()) throw new ArgumentException("Collection must containt at least one item.");

            this.CurrentQuoteCount = currentQuoteCount;
            this.Date = date;
        }
        
        /// <summary>
        /// Quote count by ticker index in this observation.
        /// </summary>
        public IReadOnlyList<int> CurrentQuoteCount { get; private set; }

        public DateTime Date { get; private set; }
    }
}
