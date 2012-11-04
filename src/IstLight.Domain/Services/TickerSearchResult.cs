using System;

namespace IstLight.Services
{
    public class TickerSearchResult
    {
        public TickerSearchResult(string tickerName, string tickerDescription)
        {
            if (string.IsNullOrWhiteSpace(tickerName))
                throw new ArgumentNullException("tickerName", "tickerName is null or empty.");

            TickerName = tickerName;
            TickerDescription = tickerDescription ?? "";
        }

        public string TickerName { get; private set; }
        public string TickerDescription { get; private set; }
    }
}
