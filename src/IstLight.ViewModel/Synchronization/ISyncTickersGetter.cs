using System;

namespace IstLight.Synchronization
{
    public interface ISyncTickersGetter
    {
        ValueOrError<Func<SyncTickers>> TryGet();
    }
}
