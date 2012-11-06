using System;

namespace IstLight.Services
{
    public class TickerSearchResult
    {
        public TickerSearchResult(string name, string description, string market)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("tickerName", "tickerName is null or empty.");

            Name = name;
            Description = description ?? "";
            Market = market ?? "";
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Market { get; private set; }
    }
}
