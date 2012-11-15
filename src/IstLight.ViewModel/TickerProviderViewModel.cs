using IstLight.Services;

namespace IstLight
{
    public class TickerProviderViewModel
    {
        private readonly ITickerProvider provider;

        public TickerProviderViewModel(ITickerProvider provider)
        {
            this.provider = provider;
        }

        public string Name { get { return provider.Name; } }
        
        internal ITickerProvider Provider { get { return provider; } }
    }
}
