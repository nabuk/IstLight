using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IstLight.Services
{
    public interface ITickerConverter : IRepositoryItem
    {
        string Format { get; }

        IAsyncResult<Ticker> Read(RawTicker rawTicker);
        IAsyncResult<RawTicker> Save(Ticker ticker);
    }
}
