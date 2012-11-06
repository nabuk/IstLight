using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IstLight
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Func<T, bool> canExecute;

        public DelegateCommand(Action<T> execute) : this(execute, null) { }
        public DelegateCommand(Action<T> execute, Func<T,bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter = null)
        {
            return canExecute == null ? true : canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void Execute(object parameter = null)
        {
            execute((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
