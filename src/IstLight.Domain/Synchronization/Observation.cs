using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
