using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace IstLight
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(TickerExplorerViewModel tickerExplorer, ErrorListViewModel errorList)
        {
            this.TickerExplorer = tickerExplorer;
            this.ErrorList = errorList;
        }

        public TickerExplorerViewModel TickerExplorer { get; private set; }
        public ErrorListViewModel ErrorList { get; private set; }
    }
}
