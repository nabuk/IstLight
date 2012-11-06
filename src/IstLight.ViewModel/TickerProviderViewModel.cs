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

        public TickerProviderViewModel(ITickerProvider provider)
        {
            this.provider = provider;
        }

        public string Name { get { return provider.Name; } }
        
        internal ITickerProvider Provider { get { return provider; } }
    }
}
