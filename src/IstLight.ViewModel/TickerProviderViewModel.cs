using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Services;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace IstLight
{
    public class TickerProviderViewModel
    {
        private readonly ITickerProvider provider;
        private readonly Action<TickerViewModel> acceptTicker;

        private readonly ObservableCollection<TickerSearchResultViewModel> searchResult = new ObservableCollection<TickerSearchResultViewModel>();

        private void Search(string hint)
        {
            //handle diff request times //DateTime.Now.Ticks
            provider.Search(hint).AddCallback(x =>
                {
                    if (x.Result != null)
                    {
                        searchResult.Clear();
                        foreach (var item in x.Result)
                            searchResult.Add(new TickerSearchResultViewModel(item));
                    }
                });
        }

        public TickerProviderViewModel(ITickerProvider provider, Action<TickerViewModel> acceptTicker)
        {
            this.provider = provider;
            this.acceptTicker = acceptTicker;
            this.SearchCommand = new RelayCommand<string>(Search, x => provider.CanSearch);
            this.DownloadCommand = new RelayCommand<string>(tName => acceptTicker(new TickerViewModel(tName, provider.Get(tName))));

            this.SearchResult = new ReadOnlyObservableCollection<TickerSearchResultViewModel>(searchResult);
        }

        public string Name { get { return provider.Name; } }

        public ICommand SearchCommand { get; private set; }
        public ICommand DownloadCommand { get; private set; }

        public ReadOnlyObservableCollection<TickerSearchResultViewModel> SearchResult { get; private set; }
    }
}
