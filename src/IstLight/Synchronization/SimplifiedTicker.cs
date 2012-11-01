using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Extensions;

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
