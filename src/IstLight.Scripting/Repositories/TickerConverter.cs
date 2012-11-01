using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight;
using IstLight.Services;

namespace IstLight.Scripting.Repositories
{
    public class TickerConverter : BaseScriptRepositoryItem, ITickerConverter
    {
        public TickerConverter(ParallelScriptExecutor executor) : base(executor) { }

        public string Format
        {
            get
            {
                var resultOrError = executor.SafeExecute<string>(engine => engine.GetVariable("Format"));
                if (resultOrError.Error != null) throw resultOrError.Error;
                return resultOrError.Result;
            }
        }

        public IAsyncResult<Ticker> Read(RawTicker rawTicker)
        {
            return executor.SafeExecuteAsync<Ticker>(engine => engine.GetVariable("Read")(rawTicker));
        }

        public IAsyncResult<RawTicker> Save(Ticker ticker)
        {
            return executor.SafeExecuteAsync<RawTicker>(engine => engine.GetVariable("Save")(ticker));
        }
    }
}
