
namespace IstLight.Services
{
    public class TickerTransformer : ScriptNamedItemBase, ITickerTransformer
    {
        public TickerTransformer(ParallelScriptExecutor executor) : base(executor) { }

        public IAsyncResult<Ticker> Transform(Ticker ticker)
        {
            return executor.SafeExecuteAsync<Ticker>(engine => engine.GetVariable("Transform")(ticker));
        }
    }
}
