using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IstLight.Commands
{
    public abstract class GlobalCommandBase : IGlobalCommand
    {
        private readonly string key;
        private readonly ICommand command;

        public GlobalCommandBase(string key, ICommand command)
        {
            this.key = key;
            this.command = command;
            this.command.CanExecuteChanged += CanExecuteChanged;
        }

        public string Key
        {
            get { return key; }
        }

        public bool CanExecute(object parameter)
        {
            return command.CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void Execute(object parameter)
        {
            command.Execute(parameter);
        }
    }
}
