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

namespace IstLight
{
    public class MultiQuoteList<T> : QuoteList<T>
        where T : IDate
    {
        public MultiQuoteList(IReadOnlyList<T> quotes, IReadOnlyList<TickerDescription> descriptions) : base(quotes)
        {
            if (descriptions == null) throw new ArgumentNullException("descriptions");

            this.Descriptions = descriptions;
        }

        public IReadOnlyList<TickerDescription> Descriptions { get; private set; }

        public int TickerCount { get { return Descriptions.Count; } }
    }

    public static class MultiQuoteListExtensions
    {
        public static int? TickerIndexByName<T>(this MultiQuoteList<T> collection, string name)
            where T : IDate
        {
            return collection.Descriptions
                .Select((d, i) => new { d.Name, i })
                .Where(d => string.Equals(d.Name, name, StringComparison.InvariantCultureIgnoreCase))
                .Select(d => (int?)d.i)
                .SingleOrDefault();
        }
    }
}
