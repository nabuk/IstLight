using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Services
{
    public interface ITickerService
    {
        IReadOnlyList<IAsyncResult<Ticker>> Load();
        IAsyncResult<bool> Save(Ticker ticker);
    }
}
