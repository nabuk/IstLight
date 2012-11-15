using IstLight.Services;

namespace IstLight
{
    public class TickerSearchResultViewModel
    {
        public TickerSearchResultViewModel(TickerSearchResult tickerSearchResult)
        {
            this.Name = tickerSearchResult.Name;
            this.Description = tickerSearchResult.Description;
            this.Market = tickerSearchResult.Market;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Market { get; private set; }
    }
}
