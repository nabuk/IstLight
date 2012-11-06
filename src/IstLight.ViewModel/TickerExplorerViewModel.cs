using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using IstLight.Services;

namespace IstLight
{
    public class TickerExplorerViewModel : ViewModelBase
    {
        public TickerExplorerViewModel(TickerProvidersViewModel providers)
        {
            this.Providers = providers;
        }

        public TickerProvidersViewModel Providers { get; private set; }
    }
}
