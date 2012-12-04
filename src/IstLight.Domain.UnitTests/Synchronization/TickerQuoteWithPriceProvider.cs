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

using IstLight.Settings;

namespace IstLight.UnitTests.Synchronization
{
    public class TickerQuoteWithPriceProvider : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (SimulationPriceType priceType in Enum.GetValues(typeof(SimulationPriceType)))
                foreach (double? volume in new double?[] { 1, null })
                    foreach (double low in new double[] { -1, 0 })
                        foreach (double high in new double[] { 0, 1 })
                            yield return new object[] { new TickerQuote(new DateTime(2000, 1, 1), high, low, high, low, volume), priceType };
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
