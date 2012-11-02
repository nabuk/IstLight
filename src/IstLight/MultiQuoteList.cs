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
