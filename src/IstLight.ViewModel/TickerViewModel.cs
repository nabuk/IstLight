using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using IstLight.Services;

namespace IstLight
{
    public class TickerViewModel : ViewModelBase
    {
        private readonly IAsyncResult<Ticker> asyncTicker;

        public TickerViewModel(string name, IAsyncResult<Ticker> asyncTicker)
        {
            this.Name = name;
            this.asyncTicker = asyncTicker;
        }

        public string Name { get; set; }
    }
}
