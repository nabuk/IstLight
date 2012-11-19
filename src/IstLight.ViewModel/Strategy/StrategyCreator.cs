using IstLight.ViewModels;

namespace IstLight.Strategy
{
    public class StrategyCreator : IStrategyCreator
    {
        private readonly StrategyExplorerViewModel strategies;
        private readonly ScriptStrategyFactory factory;

        public StrategyCreator(StrategyExplorerViewModel strategies, ScriptStrategyFactory factory)
        {
            this.strategies = strategies;
            this.factory = factory;
        }

        public StrategyBase CreateStrategy()
        {
            return factory.CreateStrategy(strategies.SelectedStrategy.ToScript());
        }
    }
}
