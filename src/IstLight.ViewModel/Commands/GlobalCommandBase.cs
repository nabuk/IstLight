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
using System.Windows.Input;

namespace IstLight.Commands
{
    public abstract class GlobalCommandBase : IGlobalCommand
    {
        private readonly string key;
        private readonly ICommand command;

        protected void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged(this, EventArgs.Empty);
        }

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
