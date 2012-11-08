using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using IstLight.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Windows.Input;

namespace IstLight
{
    public class TickerProvidersViewModel : ViewModelBase
    {
        #region Data
        private readonly IAsyncLoadService<ITickerProvider> loadProvidersService;
        private ReadOnlyObservableCollection<TickerProviderViewModel> providerVMs;
        private TickerProviderViewModel selectedProvider;
        private IEnumerable<TickerSearchResultViewModel> searchResults = Enumerable.Empty<TickerSearchResultViewModel>();
        private bool showResults = false;
        private  bool canChangeProvider = false;
        #endregion

        #region Inner methods
        private ReadOnlyObservableCollection<TickerProviderViewModel> InitializeProviders()
        {
            var observableProviders = new ThreadSafeObservableCollection<TickerProviderViewModel>();
            
            this.providerVMs = new ReadOnlyObservableCollection<TickerProviderViewModel>(observableProviders);
            (this.providerVMs as INotifyCollectionChanged).CollectionChanged += (s, e) =>
            {
                if (e.NewItems.Count > 0 && SelectedProvider == null)
                    SelectedProvider = e.NewItems[0] as TickerProviderViewModel;
                
                CanChangeProvider = providerVMs.Count > 1;
            };

            foreach (var arProvider in loadProvidersService.Load())
                arProvider.AddCallback(x =>
                    {
                        if (x.Error == null)
                            observableProviders.Add(new TickerProviderViewModel(x.Result));
                    });

            return this.providerVMs;
        }
        private void Search(string hint)
        {
            SelectedProvider.Provider.Search(hint).AddCallback(ar =>
            {
                SearchResults = (ar.Result ?? Enumerable.Empty<TickerSearchResult>()).Select(x => new TickerSearchResultViewModel(x));
                ShowSearchResults = SearchResults.Any();
            });
        }
        private TickerViewModel Download(string name)
        {
            return new TickerViewModel(name, SelectedProvider.Provider.Get(name));
        }
        private void HandleSelectedProviderChange(TickerProviderViewModel previous, TickerProviderViewModel current)
        {
            if ((previous != null && previous.Provider.CanSearch) != (current != null && current.Provider.CanSearch))
            {
                base.RaisePropertyChanged<bool>(() => CanSearch);
                (SearchCommand as DelegateCommand<string>).RaiseCanExecuteChanged();
            }

            if ((previous == null) != (current == null))
            {
                base.RaisePropertyChanged<bool>(() => CanDownload);
                (DownloadCommand as DelegateCommand<string>).RaiseCanExecuteChanged();
            }

            base.RaisePropertyChanged<TickerProviderViewModel>(() => SelectedProvider);
        }
        #endregion

        public TickerProvidersViewModel(IAsyncLoadService<ITickerProvider> loadProvidersService, Action<TickerViewModel> acceptTicker)
        {
            this.loadProvidersService = loadProvidersService;

            SearchCommand = new DelegateCommand<string>(hint => Search(hint), x => CanSearch);
            DownloadCommand = new DelegateCommand<string>(name => acceptTicker(Download(name)), x => CanDownload);
        }

        public ReadOnlyObservableCollection<TickerProviderViewModel> Providers
        {
            get
            {
                return providerVMs ?? (providerVMs = InitializeProviders());
            }
        }
        public TickerProviderViewModel SelectedProvider
        {
            get { return selectedProvider; }
            set
            {
                if (value == selectedProvider)
                    return;

                var previous = SelectedProvider;
                this.selectedProvider = value;
                HandleSelectedProviderChange(previous, value);
            }
        }
        public bool CanSearch { get { return SelectedProvider == null ? false : SelectedProvider.Provider.CanSearch; } }
        public bool CanDownload { get { return SelectedProvider != null; } }
        public ICommand SearchCommand { get; private set; }
        public ICommand DownloadCommand { get; private set; }
        public bool CanChangeProvider
        {
            get { return canChangeProvider; }
            private set
            {
                if (value == canChangeProvider) return;
                canChangeProvider = value;
                base.RaisePropertyChanged<bool>(() => CanChangeProvider);
            }
        }
        public IEnumerable<TickerSearchResultViewModel> SearchResults
        {
            get
            {
                return searchResults;
            }
            private set
            {
                searchResults = value;
                base.RaisePropertyChanged<IEnumerable<TickerSearchResultViewModel>>(() => SearchResults);
            }
        }

        public bool ShowSearchResults
        {
            get { return this.showResults; }
            set
            {
                if (showResults == value) return;
                this.showResults = value;
                base.RaisePropertyChanged<bool>(() => ShowSearchResults);
            }
        }

    }
}