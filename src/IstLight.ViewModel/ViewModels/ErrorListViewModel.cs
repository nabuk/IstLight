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
using System.Collections.Specialized;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using IstLight.Services;

namespace IstLight.ViewModels
{
    public class ErrorListViewModel : ViewModelBase, IErrorReporter
    {
        private readonly ThreadSafeObservableCollection<string> errorList = new ThreadSafeObservableCollection<string>();
        
        public ErrorListViewModel()
        {
            this.ClearCommand = new DelegateCommand(() => errorList.Clear(), () => ErrorList.Count > 0);
            this.ErrorList = new ReadOnlyObservableCollection<string>(errorList);
            (this.ErrorList as INotifyCollectionChanged).CollectionChanged += delegate { (ClearCommand as DelegateCommand).RaiseCanExecuteChanged(); };
            (this.ErrorList as INotifyCollectionChanged).CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                    foreach (string error in e.NewItems)
                        NewError(this,EventArgs.Empty);
            };
        }

        public ReadOnlyObservableCollection<string> ErrorList
        {
            get;
            private set;
        }

        public event EventHandler NewError = delegate { };

        public ICommand ClearCommand { get; private set; }

        void IErrorReporter.Add(string error)
        {
            (this as IErrorReporter).Add(new Exception(error));
        }

        void IErrorReporter.Add(Exception error)
        {
            StringBuilder sb = new StringBuilder();
            bool topLevel = true;

            while (error != null)
            {
                if (!topLevel)
                {
                    sb.AppendLine();
                    sb.AppendLine("Inner exception:");
                }
                var exType = error.GetType();
                if (exType != typeof(Exception) || !topLevel)
                    sb.AppendLine("Type: " + exType.Name);

                sb.AppendLine(error.Message);

                if (error.StackTrace != null)
                {
                    sb.AppendLine("StackTrace:");
                    sb.AppendLine(error.StackTrace);
                }

                error = error.InnerException;
            }

            errorList.Add(sb.ToString().Trim());
        }
    }
}
