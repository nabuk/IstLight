
namespace IstLight.Services.Decorators
{
    public class TickerProviderErrorDecorator : NamedItemBaseErrorDecorator<ITickerProvider>, ITickerProvider
    {
        public TickerProviderErrorDecorator(ITickerProvider itemToDecorate, IErrorReporter errorReporter)
            : base(itemToDecorate, errorReporter) { }

        #region ITickerProvider
        public bool CanSearch
        {
            get { return base.itemToDecorate.CanSearch; }
        }

        public IAsyncResult<IReadOnlyList<TickerSearchResult>> Search(string hint)
        {
            return Decorate(itemToDecorate.Search(hint));
        }

        public IAsyncResult<Ticker> Get(string tickerName)
        {
            return Decorate(itemToDecorate.Get(tickerName));
        }
        #endregion //ITickerProvider
    }
}
