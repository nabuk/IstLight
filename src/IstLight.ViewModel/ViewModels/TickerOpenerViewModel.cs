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
using System.Threading.Tasks;
using WF = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using IstLight.Services;
using System.IO;
using System.Threading;

namespace IstLight.ViewModels
{
    public class TickerOpenerViewModel
    {
        private readonly Dispatcher dispatcher;
        private ITickerConverter[] converters = new ITickerConverter[0];
        private readonly IFileIO fileIO;

        private string Filter
        {
            get
            {
                return converters
                    .Select(conv =>
                        string.Format("{0} files ({1})|{1}",
                            conv.Name,
                            conv.Format.ToUpper()
                                .Split(';')
                                .Select(ext => "*."+ext)
                                .Aggregate((e1,e2) => e1+";"+e2)))
                    .Aggregate((s1,s2) => s1 + "|" + s2);
            }
        }

        private void Open()
        {
            var openDlg = new WF.OpenFileDialog
            {
                Multiselect = true,
                CheckFileExists = true,
                AddExtension = true,
                Filter = Filter,
            };
            if (openDlg.ShowDialog() == WF.DialogResult.OK)
            {
                Task.Factory.StartNew(paths =>
                    {
                        var toLoad = ((string[])paths)
                            .Select(p => new KeyValuePair<string,ResultAsAsyncResult<Ticker>>(p, new ResultAsAsyncResult<Ticker>()))
                            .ToArray();

                        dispatcher.InvokeIfRequired(() =>
                            {
                                foreach(var x in toLoad)
                                    LoadingTicker(
                                        new TickerFileViewModel(Path.GetFileNameWithoutExtension(x.Key),x.Value));
                            });

                        toLoad.AsParallel().ForAll(x =>
                            {
                                var rawFile = fileIO.Read(x.Key);
                                var readResult = rawFile != null ?
                                    converters[openDlg.FilterIndex - 1].Read(rawFile).Sync()
                                    :
                                    new ValueOrError<Ticker> { Error = new Exception("Cannot read " + x.Key) };
                                x.Value.SetResult(readResult);
                            });


                        //foreach (var filePath in (string[])paths)
                        //{
                        //    //var rawFile = fileIO.Read(filePath);
                        //    //var readResult = rawFile != null ?
                        //    //    converters[openDlg.FilterIndex - 1].Read(rawFile).Sync()
                        //    //    :
                        //    //    new ValueOrError<Ticker> { Error = new Exception("Cannot read " + filePath) };
                        //    var readResult = new ValueOrError<Ticker> { Error = new Exception("Cannot read " + filePath) };

                        //    dispatcher.InvokeIfRequired(() =>
                        //        LoadingTicker(
                        //            new TickerFileViewModel(
                        //                Path.GetFileNameWithoutExtension(filePath),
                        //                new ResultAsAsyncResult<Ticker>(readResult))));
                        //}
                        
                    }, openDlg.FileNames);
            }
        }

        private void BeginLoadConverters(IAsyncLoadService<ITickerConverter> loadConvertersService)
        {
            Task.Factory.StartNew(() =>
                {
                    converters =
                            loadConvertersService
                                .Load()
                                .Select(x => { x.Wait(System.Threading.Timeout.Infinite); return x.Result; })
                                .Where(x => x != null)
                                .ToArray();
                    dispatcher.InvokeIfRequired(((DelegateCommand)OpenCommand).RaiseCanExecuteChanged);
                });
        }

        public TickerOpenerViewModel(IAsyncLoadService<ITickerConverter> loadConvertersService, IFileIO fileIO)
        {
            this.dispatcher = Dispatcher.CurrentDispatcher;
            this.fileIO = fileIO;
            this.OpenCommand = new DelegateCommand(Open, () => converters.Length > 0);

            BeginLoadConverters(loadConvertersService);
        }

        public ICommand OpenCommand { get; private set; }

        public event Action<TickerFileViewModel> LoadingTicker = delegate { };
    }
}
