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
                    .Select(conv => string.Format("{0} files (*.{1})|*.{1}", conv.Name, conv.Format.ToUpper()))
                    .Aggregate((s1,s2) => s1 + "|" + s2);
            }
        }
        private void ReadTicker(string filePath, ITickerConverter converter)
        {
            LoadingTicker(
                new TickerFileViewModel(
                    Path.GetFileNameWithoutExtension(filePath),
                    new AsyncResultFromSyncJob<Ticker>(() =>
                        {
                            var tickerFile = fileIO.Read(filePath);
                            return tickerFile != null ?
                                converter.Read(tickerFile).Sync()
                                :
                                new ValueOrError<Ticker> { Error = new Exception("Cannot read " + filePath) };
                        })));
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
                foreach (var filePath in openDlg.FileNames)
                    ReadTicker(filePath, converters[openDlg.FilterIndex-1]);
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
