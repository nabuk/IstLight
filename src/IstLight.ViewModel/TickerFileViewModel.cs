using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using IstLight.Services;

namespace IstLight
{
    public class TickerFileViewModel : ViewModelBase
    {
        private readonly IAsyncResult<Ticker> asyncTicker;
        private readonly Dispatcher dispatcher;

        private void HandleLoaded()
        {
            dispatcher.InvokeIfRequired(() =>
            {
                LoadState = asyncTicker.GetState();
                base.RaisePropertyChanged<AsyncState>(() => LoadState);

                //if (asyncTicker.Error != null)
                //    CloseCommand.Execute(null);
            });
        }

        public TickerFileViewModel(string name, IAsyncResult<Ticker> asyncTicker)
        {
            this.dispatcher = Dispatcher.CurrentDispatcher;
            this.Name = name;
            this.asyncTicker = asyncTicker;
            asyncTicker.AddCallback(x => HandleLoaded());
            this.CloseCommand = new DelegateCommand(() => CloseCommandExecuted(this));
        }

        public string Name { get; set; }

        public ICommand CloseCommand { get; private set; }
        public event Action<TickerFileViewModel> CloseCommandExecuted = delegate { };

        public AsyncState LoadState
        {
            get;
            private set;
        }

    }
}
