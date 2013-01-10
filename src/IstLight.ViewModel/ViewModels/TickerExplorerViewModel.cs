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

using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace IstLight.ViewModels
{
    public class TickerExplorerViewModel : ViewModelBase
    {
        private readonly ObservableCollection<TickerFileViewModel> tickers = new ObservableCollection<TickerFileViewModel>();
        private readonly object recalculateIndicesSync = new object();

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

        private void HandleTickerLoading(TickerFileViewModel tickerVM)
        {
            tickers.Add(tickerVM);
            tickerVM.ExecuteWhenLoadCompletes(x => RecalculateIndicesFrom(tickers.IndexOf(x)));
            tickerVM.CloseCommandExecuted += x =>
            {
                int index = tickers.IndexOf(x);
                tickers.Remove(x);
                RecalculateIndicesFrom(index);
            };
        }
        
        public TickerExplorerViewModel(TickerProvidersViewModel providers, TickerOpenerViewModel opener)
        {
            this.Providers = providers;
            this.Opener = opener;
            this.Tickers = new ReadOnlyObservableCollection<TickerFileViewModel>(this.tickers);
            Providers.LoadingTicker += HandleTickerLoading;
            Opener.LoadingTicker += HandleTickerLoading;
        }

        public TickerProvidersViewModel Providers { get; private set; }
        public TickerOpenerViewModel Opener { get; private set; }
        public ReadOnlyObservableCollection<TickerFileViewModel> Tickers { get; private set; }
    }
}
