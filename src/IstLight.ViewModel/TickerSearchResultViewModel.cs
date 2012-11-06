using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Services;

namespace IstLight
{
    public class TickerSearchResultViewModel
    {
        public TickerSearchResultViewModel(TickerSearchResult tickerSearchResult)
        {
            this.TickerName = tickerSearchResult.TickerName;
            this.TickerDescription = tickerSearchResult.TickerDescription;
        }

        public string TickerName { get; private set; }
        public string TickerDescription { get; private set; }
    }
}
