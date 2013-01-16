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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using IstLight.Services;
using IstLight.Simulation;

namespace IstLight.ViewModels.ResultSections
{
    public class SummaryGroupViewModel : ViewModelBase
    {
        private readonly IAsyncResult<IResultAnalyzer> analyzer;
        private readonly SimulationResult result;
        private readonly Dispatcher dispatcher;

        private string name;
        private IEnumerable<KeyValuePair<string, string>> items;
        private AsyncState loadState;

        private void HandleAnalyzerLoaded()
        {
            dispatcher.InvokeIfRequired(() =>
            {
                if (analyzer.Error != null)
                {
                    Name = "Can't load";
                    LoadState = AsyncState.Error;
                    return;
                }

                Name = analyzer.Result.Category;
                analyzer.Result.Analyze(result).AddCallback(HandleAnalyzeResult);
            });
        }
        private void HandleAnalyzeResult(IAsyncResult<IReadOnlyList<KeyValuePair<string,string>>> analyzeResult)
        {
            dispatcher.InvokeIfRequired(() =>
            {
                if (analyzeResult.Error != null)
                {
                    Name = string.Format("{0} - can't analyze",analyzer.Result.Category);
                    LoadState = AsyncState.Error;
                    return;
                }

                Items = analyzeResult.Result;
                LoadState = AsyncState.Completed;
            });
        }

        public SummaryGroupViewModel(IAsyncResult<IResultAnalyzer> analyzer, SimulationResult result)
        {
            this.Name = "Loading ...";
            this.LoadState = AsyncState.Running;
            this.analyzer = analyzer;
            this.result = result;
            this.dispatcher = Dispatcher.CurrentDispatcher;
            
            analyzer.AddCallback(x => HandleAnalyzerLoaded());
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                base.RaisePropertyChanged<string>(() => Name);
            }
        }
        public IEnumerable<KeyValuePair<string, string>> Items
        {
            get { return items ?? Enumerable.Empty<KeyValuePair<string,string>>(); }
            set
            {
                items = value;
                base.RaisePropertyChanged<IEnumerable<KeyValuePair<string, string>>>(() => Items);
            }
        }
        public AsyncState LoadState
        {
            get { return loadState; }
            private set
            {
                loadState = value;
                base.RaisePropertyChanged<AsyncState>(() => LoadState);
            }
        }
    }
}