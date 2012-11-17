using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using IstLight.Services;

namespace IstLight
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
