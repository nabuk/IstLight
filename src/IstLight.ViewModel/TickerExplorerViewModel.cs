using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            tickers.CollectionChanged += (s, e) =>
            {
                var collection = s as ObservableCollection<TickerFileViewModel>;
                if (e.Action == NotifyCollectionChangedAction.Remove)
                    for (int i = e.OldStartingIndex; i < collection.Count; i++)
                        collection[i].Index = i;
            };
            Providers.LoadingTicker += tvm =>
            {
                tvm.Index = tickers.Count;
                tickers.Add(tvm);
                tvm.CloseCommandExecuted += x => tickers.Remove(x);
            };
        }

        public TickerProvidersViewModel Providers { get; private set; }

        public ReadOnlyObservableCollection<TickerFileViewModel> Tickers { get; private set; }
    }
}
