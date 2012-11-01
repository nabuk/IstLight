using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IstLight.Services
{
    public interface ITickerTransformer : IRepositoryItem
    {
        IAsyncResult<Ticker> Transform(Ticker ticker);
    }
}
