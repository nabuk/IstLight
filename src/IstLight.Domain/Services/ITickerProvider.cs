using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IstLight.Services
{
    public interface ITickerProvider : IRepositoryItem
    {
        bool CanSearch { get; }
        IAsyncResult<IReadOnlyList<TickerSearchResult>> Search(string hint);
        IAsyncResult<Ticker> Get(string tickerName);
    }
}
