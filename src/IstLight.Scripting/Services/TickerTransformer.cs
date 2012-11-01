using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight;
using IstLight.Services;

namespace IstLight.Scripting.Services
{
    public class TickerTransformer : BaseScriptServiceItem, ITickerTransformer
    {
        public TickerTransformer(ParallelScriptExecutor executor) : base(executor) { }

        public IAsyncResult<Ticker> Transform(Ticker ticker)
        {
            return executor.SafeExecuteAsync<Ticker>(engine => engine.GetVariable("Transform")(ticker));
        }
    }
}
