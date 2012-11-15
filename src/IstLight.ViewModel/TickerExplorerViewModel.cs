using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace IstLight
{
    public class TickerExplorerViewModel : ViewModelBase
    {
        private readonly ObservableCollection<TickerFileViewModel> tickers = new ObservableCollection<TickerFileViewModel>();
        private readonly object recalculateIndicesSync = new object();

        private void RecalculateIndicesFrom(int current)
        {
            lock (recalculateIndicesSync)
            {
                if (current >= tickers.Count)
                    return;

                int indexBefore = -1;
                for (int i = current-1; i >= 0; i--)
                    if (tickers[i].Index != null)
                    {
                        indexBefore = tickers[i].Index.Value;
                        break;
                    }

                for (; current < tickers.Count; current++)
                    if (tickers[current].LoadState == AsyncState.Completed)
                        tickers[current].Index = ++indexBefore;
            }
        }

        private void HandleTickerLoading(TickerFileViewModel tickerVM)
        {
            tickers.Add(tickerVM);
            tickerVM.ExecuteWhenLoadCompletes(x => RecalculateIndicesFrom(tickers.IndexOf(x)));
            tickerVM.CloseCommandExecuted += x =>
            {
                int index = tickers.IndexOf(x);
                tickers.Remove(x);
                RecalculateIndicesFrom(index);
            };
        }
        
        public TickerExplorerViewModel(TickerProvidersViewModel providers)
        {
            this.Providers = providers;
            this.Tickers = new ReadOnlyObservableCollection<TickerFileViewModel>(this.tickers);
            Providers.LoadingTicker += HandleTickerLoading;
        }

        public TickerProvidersViewModel Providers { get; private set; }
        public ReadOnlyObservableCollection<TickerFileViewModel> Tickers { get; private set; }
    }
}
