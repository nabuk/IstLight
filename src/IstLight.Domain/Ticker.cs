using System;
using System.Linq;
using IstLight.Domain.Services;

namespace IstLight.Domain
{
    public class Ticker : QuoteList<ITickerQuote>
    {
        public Ticker(string name, IReadOnlyList<ITickerQuote> quotes) : base(quotes)
        {
            if (name == null) throw new ArgumentNullException("name");

            this.Name = name;
        }

        public string Name { get; set; }

        public bool CanBeBought { get { return this.All(q => q.Low > 0); } }
    }
}
