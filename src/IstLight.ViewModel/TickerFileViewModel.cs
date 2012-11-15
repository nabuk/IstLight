﻿using System;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using IstLight.Services;

namespace IstLight
{
    public class TickerFileViewModel : ViewModelBase
    {
        private readonly Dispatcher dispatcher;
        private int? index;
        internal readonly IAsyncResult<Ticker> AsyncTicker;
        private readonly OneTimeCallbackContainer<TickerFileViewModel> completionCallbacks = new OneTimeCallbackContainer<TickerFileViewModel>();

        private void HandleLoaded()
        {
            dispatcher.InvokeIfRequired(() =>
            {
                LoadState = AsyncTicker.GetState();
                base.RaisePropertyChanged<AsyncState>(() => LoadState);

                if (AsyncTicker.Error == null)
                {
                    Func<DateTime, string> formatDate = dt => dt.ToShortDateString();
                    var t = AsyncTicker.Result;
                    From = formatDate(t.From); base.RaisePropertyChanged<string>(() => From);
                    To = formatDate(t.To); base.RaisePropertyChanged<string>(() => To);
                    completionCallbacks.FireCallbacks(this);
                }
            });
        }

        public TickerFileViewModel(string name, IAsyncResult<Ticker> asyncTicker)
        {
            this.dispatcher = Dispatcher.CurrentDispatcher;
            this.Name = name;
            this.AsyncTicker = asyncTicker;
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
        public void ExecuteWhenLoadCompletes(Action<TickerFileViewModel> callback)
        {
            completionCallbacks.AddCallback(callback);
            if (LoadState == AsyncState.Completed)
                completionCallbacks.FireCallbacks(this);
        }

        public int? Index
        {
            get { return index; }
            internal set
            {
                index = value;
                base.RaisePropertyChanged<int?>(() => Index);
            }
        }

        public string From { get; private set; }
        public string To { get; private set; }
    }
}
