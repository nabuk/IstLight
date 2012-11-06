using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace IstLight
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(TickerExplorerViewModel tickerExplorer)
        {
            this.TickerExplorer = tickerExplorer;
        }


        public TickerExplorerViewModel TickerExplorer { get; private set; }
    }
}
