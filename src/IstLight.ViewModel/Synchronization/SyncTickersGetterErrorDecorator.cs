using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Services;

namespace IstLight.Synchronization
{
    public class SyncTickersGetterErrorDecorator : ISyncTickersGetter
    {
        private readonly ISyncTickersGetter syncTickersFactory;
        private readonly IErrorReporter errorReporter;

        public SyncTickersGetterErrorDecorator(ISyncTickersGetter syncTickersFactory, IErrorReporter errorReporter)
        {
            this.syncTickersFactory = syncTickersFactory;
            this.errorReporter = errorReporter;
        }

        ValueOrError<Func<SyncTickers>> ISyncTickersGetter.TryGet()
        {
            var valueOrError = syncTickersFactory.TryGet();
            errorReporter.AddIfNotNull(valueOrError.Error);
            return valueOrError;
        }
    }
}
