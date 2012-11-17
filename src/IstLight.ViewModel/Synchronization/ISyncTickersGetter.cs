using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Synchronization
{
    public interface ISyncTickersGetter
    {
        ValueOrError<Func<SyncTickers>> TryGet();
    }
}
