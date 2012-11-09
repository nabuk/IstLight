using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using IstLight.Services;

namespace IstLight
{
    public class TickerExplorerViewModel : ViewModelBase
    {
        private readonly ObservableCollection<TickerFileViewModel> tickers = new ObservableCollection<TickerFileViewModel>();
        
        public TickerExplorerViewModel(TickerProvidersViewModel providers)
        {
            this.Providers = providers;
            this.Tickers = new ReadOnlyObservableCollection<TickerFileViewModel>(this.tickers);
            Providers.LoadingTicker += tvm =>
                {
                    tickers.Add(tvm);
                    tvm.CloseCommandExecuted += x => tickers.Remove(x);
                };
        }

        public TickerProvidersViewModel Providers { get; private set; }

        public ReadOnlyObservableCollection<TickerFileViewModel> Tickers { get; private set; }
    }
}
