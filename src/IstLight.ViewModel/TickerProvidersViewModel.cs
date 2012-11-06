using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using IstLight.Services;
using System.Collections.ObjectModel;

namespace IstLight
{
    public class TickerProvidersViewModel : ViewModelBase
    {
        private readonly IAsyncLoadValidService<ITickerProvider> loadProvidersService;
        private readonly Action<TickerViewModel> acceptTicker;
        private ReadOnlyObservableCollection<TickerProviderViewModel> providerVMs;
        private readonly ThreadSafeObservableCollection<TickerProviderViewModel> observableProviders = new ThreadSafeObservableCollection<TickerProviderViewModel>();
        private TickerProviderViewModel selectedProvider;

        public TickerProvidersViewModel(IAsyncLoadValidService<ITickerProvider> loadProvidersService, Action<TickerViewModel> acceptTicker)
        {
            this.loadProvidersService = loadProvidersService;
            this.acceptTicker = acceptTicker;
            this.observableProviders.CollectionChanged += (s, e) =>
            {
                if (e.NewItems.Count > 0 && SelectedProvider == null)
                    SelectedProvider = e.NewItems[0] as TickerProviderViewModel;
            };
        }

        public ReadOnlyObservableCollection<TickerProviderViewModel> Providers
        {
            get
            {
                if (providerVMs == null)
                {
                    providerVMs = new ReadOnlyObservableCollection<TickerProviderViewModel>(observableProviders);
                    loadProvidersService.AttachCallback(iProvider =>
                            observableProviders.Add(new TickerProviderViewModel(iProvider, acceptTicker)));
                    loadProvidersService.Load();
                }

                return providerVMs;
            }
        }

        public TickerProviderViewModel SelectedProvider
        {
            get { return selectedProvider; }
            set
            {
                if (value == selectedProvider)
                    return;

                selectedProvider = value;
                base.RaisePropertyChanged<TickerProviderViewModel>(() => SelectedProvider);
            }
        }
    }
}
