
namespace IstLight.Services
{
    public class TickerConverter : ScriptNamedItemBase, ITickerConverter
    {
        public TickerConverter(string format, ParallelScriptExecutor executor) : base(executor)
        {
            this.Format = format;
        }

        public string Format
        {
            get;
            private set;
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
