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
