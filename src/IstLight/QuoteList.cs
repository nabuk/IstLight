using System;
using System.Collections.Generic;
using System.Linq;

namespace IstLight
{
    /// <summary>
    /// Base class for ticker collections.
    /// </summary>
    /// <typeparam name="T">Type of quote.</typeparam>
    public class QuoteList<T> : IReadOnlyList<T>, IEnumerable<T>
        where T : IDate
    {
        public QuoteList(IReadOnlyList<T> quotes)
        {
            if (quotes == null) throw new ArgumentNullException("quotes");
            if (quotes.Count < 1) throw new ArgumentException("Collection must containt at least one item.");
            if (quotes.Any(q => q == null)) throw new ArgumentException("One of the quotes was null.");

            if (quotes.Count > 1)
                switch (CheckOrder(quotes))
                {
                    case Order.Asc: break;
                    case Order.Desc: quotes = quotes.Reverse(); break;
                    case Order.Unordered: throw new ArgumentException("Collection is unordered.");
                    case Order.Duplication: throw new ArgumentException("Quote dates are duplicated.");
                }

            this.quotes = quotes;
        }

        /// <summary>
        /// First date.
        /// </summary>
        public DateTime From { get { return quotes[0].Date; } }

        /// <summary>
        /// Last date.
        /// </summary>
        public DateTime To { get { return quotes[quotes.Count - 1].Date; } }

        #region Helpers

        [Flags]
        private enum Order
        {
            Unordered = 0,
            Asc = 1,
            Desc = 2,
            Duplication = 4
        }

        private Order CheckOrder(IReadOnlyList<T> quotes)
        {
            Order hash = Order.Asc | Order.Desc;

            foreach(var qpair in quotes.Zip(quotes.Skip(1), (q1,q2) => new { Date1 = q1.Date, Date2 = q2.Date }))
            {
                if (qpair.Date1 == qpair.Date2) return Order.Duplication;

                hash &= qpair.Date2 > qpair.Date1 ? Order.Asc : Order.Desc;
                
                if (hash == Order.Unordered) return Order.Unordered;
            }

            return hash;
        }

        #endregion //Helpers

        #region IReadOnlyList
        public virtual T this[int index]
        {
            get { return quotes[index]; }
        }

        public int Count
        {
            get { return quotes.Count; }
        }

        private readonly IReadOnlyList<T> quotes;
        #endregion //IReadOnlyList

        #region IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            return quotes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return quotes.GetEnumerator();
        }
        #endregion //IEnumerable
    }
}
