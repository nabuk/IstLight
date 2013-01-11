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
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace IstLight.ViewModels
{
    public class TickerExplorerViewModel : ViewModelBase
    {
        private readonly ObservableCollection<TickerFileViewModel> tickers = new ObservableCollection<TickerFileViewModel>();
        private readonly object recalculateIndicesSync = new object();

        private int loadingCount = 0;
        private int errorCount = 0;
        private int loadedCount = 0;

        private void ChangeCount(AsyncState state, bool increment)
        {
            int v = increment ? 1 : -1;
            switch (state)
            {
                case AsyncState.Running: LoadingCount += v; break;
                case AsyncState.Error: ErrorCount += v; break;
                case AsyncState.Completed: LoadedCount += v; break;
                default: throw new NotImplementedException(state.ToString());
            }
        }
        private void RecalculateIndicesFrom(int current)
        {
            lock (recalculateIndicesSync)
            {
                if (current >= tickers.Count || current < 0)
                    return;

                int indexBefore = -1;
                for (int i = current-1; i >= 0; i--)
                    if (tickers[i].Index != null)
                    {
                        indexBefore = tickers[i].Index.Value;
                        break;
                    }

                for (; current < tickers.Count; current++)
                    if (tickers[current].LoadState == AsyncState.Completed)
                        tickers[current].Index = ++indexBefore;
            }
        }

        private void OnTickerLoading(TickerFileViewModel tickerVM)
        {
            LoadingCount++;

            tickers.Add(tickerVM);
            tickerVM.ExecuteWhenLoadCompletes(x => { if (x.LoadState == AsyncState.Error)OnTickerError(x); else OnTickerLoaded(x); });
            tickerVM.CloseCommandExecuted += OnCloseCommand;
        }
        private void OnTickerError(TickerFileViewModel tickerVM)
        {
            LoadingCount--;
            ErrorCount++;
        }

        private void OnTickerLoaded(TickerFileViewModel tickerVM)
        {
            LoadingCount--;
            LoadedCount++;
            RecalculateIndicesFrom(tickers.IndexOf(tickerVM));
        }
        private void OnCloseCommand(TickerFileViewModel tickerVM)
        {
            ChangeCount(tickerVM.LoadState, false);
            
            int index = tickers.IndexOf(tickerVM);
            tickers.Remove(tickerVM);
            RecalculateIndicesFrom(index);
        }

        public TickerExplorerViewModel(TickerProvidersViewModel providers, TickerOpenerViewModel opener)
        {
            this.Providers = providers;
            this.Opener = opener;
            this.Tickers = new ReadOnlyObservableCollection<TickerFileViewModel>(this.tickers);
            Providers.LoadingTicker += OnTickerLoading;
            Opener.LoadingTicker += OnTickerLoading;
        }

        public TickerProvidersViewModel Providers { get; private set; }
        public TickerOpenerViewModel Opener { get; private set; }
        public ReadOnlyObservableCollection<TickerFileViewModel> Tickers { get; private set; }

        public int LoadingCount
        {
            get { return loadingCount; }
            private set
            {
                if (value == LoadingCount)
                    return;

                loadingCount = value;
                RaisePropertyChanged<int>(() => LoadingCount);
            }
        }
        public int ErrorCount
        {
            get { return errorCount; }
            private set
            {
                if (value == ErrorCount)
                    return;

                errorCount = value;
                RaisePropertyChanged<int>(() => ErrorCount);
            }
        }
        public int LoadedCount
        {
            get { return loadedCount; }
            private set
            {
                if (value == LoadedCount)
                    return;

                loadedCount = value;
                RaisePropertyChanged<int>(() => LoadedCount);
            }
        }
    }
}
