using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using IstLight.Services;

namespace IstLight.ViewModels
{
    public class TickerProvidersViewModel : ViewModelBase
    {
        #region Data
        private readonly IAsyncLoadService<ITickerProvider> loadProvidersService;
        private ReadOnlyObservableCollection<TickerProviderViewModel> providerVMs;
        private TickerProviderViewModel selectedProvider;
        private IEnumerable<TickerSearchResultViewModel> searchResults = Enumerable.Empty<TickerSearchResultViewModel>();
        private bool showResults = false;
        private bool blockShowResultsTillNextSearch = false;
        private  bool canChangeProvider = false;
        private string hint = string.Empty;
        private ConcurrentDictionary<object, long> searchRequestTime = new ConcurrentDictionary<object, long>();
        private long lastReqTime = 0;
        private readonly object syncUpdateLastReqTime = new object();
        #endregion

        #region Inner methods
        private bool IsLastRequest(long reqTime)
        {
            lock (syncUpdateLastReqTime)
            {
                if (reqTime > lastReqTime)
                {
                    lastReqTime = reqTime;
                    return true;
                }
                else
                    return false;
            }
        }

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
            long currentTicks = DateTime.Now.Ticks;
            hint = RelevantText(hint);
            if (hint == string.Empty)
            {
                IsLastRequest(currentTicks);
                ShowSearchResults = false;
                return;
            }
            blockShowResultsTillNextSearch = false;

            var arReq = SelectedProvider.Provider.Search(hint);
            searchRequestTime.AddOrUpdate(arReq, currentTicks,(k,v)=>v);
            arReq.AddCallback(ar =>
            {
                long reqTime = 0;
                searchRequestTime.TryRemove(ar, out reqTime);
                if (IsLastRequest(reqTime))
                {
                    SearchResults = (ar.Result ?? Enumerable.Empty<TickerSearchResult>()).Select(x => new TickerSearchResultViewModel(x));
                    ShowSearchResults = SearchResults.Any();
                }
            });
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
        private void HandleDownloadCommand(string tickerName)
        {
            if (!string.IsNullOrWhiteSpace(tickerName))
            {
                tickerName = RelevantText(tickerName);
                blockShowResultsTillNextSearch = true;
                ShowSearchResults = false;
                Hint = string.Empty;
                LoadingTicker(new TickerFileViewModel(tickerName,SelectedProvider.Provider.Get(tickerName)));
            }
        }

        private string RelevantText(string str)
        {
            return (str ?? "").Trim().ToUpperInvariant();
        }
        #endregion

        public TickerProvidersViewModel(IAsyncLoadService<ITickerProvider> loadProvidersService)
        {
            this.loadProvidersService = loadProvidersService;

            SearchCommand = new DelegateCommand<string>(hint => Search(hint), x => CanSearch);
            DownloadCommand = new DelegateCommand<string>(HandleDownloadCommand, x => CanDownload);
            CloseSearchResultsCommand = new DelegateCommand(() => ShowSearchResults = false);
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
                if (value && blockShowResultsTillNextSearch) return;
                this.showResults = value;
                base.RaisePropertyChanged<bool>(() => ShowSearchResults);
            }
        }
        public ICommand CloseSearchResultsCommand { get; private set; }

        public event Action<TickerFileViewModel> LoadingTicker = delegate { };

        public string Hint
        {
            get { return hint; }
            set
            {
                if (hint == value)
                    return;

                hint = value;
                base.RaisePropertyChanged<string>(() => Hint);
                if (CanSearch) SearchCommand.Execute(Hint);
            }
        }
    }
}