// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using IstLight.Services;

namespace IstLight.ViewModels
{
    public class TickerFileViewModel : ViewModelBase
    {
        private readonly Dispatcher dispatcher;
        private int? index;
        private readonly IAsyncResult<Ticker> AsyncTicker;
        private readonly OneTimeCallbackContainer<TickerFileViewModel> completionCallbacks = new OneTimeCallbackContainer<TickerFileViewModel>();
        private readonly object stateChangeSync = new object();

        private void HandleLoaded()
        {
            dispatcher.InvokeIfRequired(() =>
            {
                lock(stateChangeSync)
                    LoadState = AsyncTicker.GetState();
                base.RaisePropertyChanged<AsyncState>(() => LoadState);

                if (AsyncTicker.Error == null)
                {
                    Func<DateTime, string> formatDate = dt => dt.ToShortDateString();
                    var t = AsyncTicker.Result;
                    From = formatDate(t.From); base.RaisePropertyChanged<string>(() => From);
                    To = formatDate(t.To); base.RaisePropertyChanged<string>(() => To);
                }

                completionCallbacks.FireCallbacks(this);
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
            AsyncState state;
            lock (stateChangeSync)
                state = LoadState;
            if(state == AsyncState.Completed)
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

        public Ticker Ticker
        {
            get
            {
                var ticker = AsyncTicker.Result;
                if (ticker == null)
                    throw new InvalidOperationException("Ticker is not loaded.");
                ticker.Name = Name;
                return ticker;
            }
        }
    }
}
