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