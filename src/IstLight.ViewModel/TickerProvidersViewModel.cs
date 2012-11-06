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

        public TickerProvidersViewModel(IAsyncLoadValidService<ITickerProvider> loadProvidersService, Action<TickerViewModel> acceptTicker)
        {
            this.loadProvidersService = loadProvidersService;
            this.acceptTicker = acceptTicker;
        }

        public ReadOnlyObservableCollection<TickerProviderViewModel> Providers
        {
            get
            {
                if (providerVMs == null)
                {
                    var observableProviders = new ThreadSafeObservableCollection<TickerProviderViewModel>();
                    providerVMs = new ReadOnlyObservableCollection<TickerProviderViewModel>(observableProviders);
                    loadProvidersService.AttachCallback(iProvider =>
                            observableProviders.Add(new TickerProviderViewModel(iProvider, acceptTicker)));
                    loadProvidersService.Load();
                }

                return providerVMs;
            }
        }
    }
}
