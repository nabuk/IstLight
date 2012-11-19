using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace IstLight
{
    public class PopupWindowBase : Window
    {
        public PopupWindowBase()
        {
            MoveWindowCommand = new DelegateCommand(() => this.DragMove());
        }

        public ICommand MoveWindowCommand { get; private set; }
    }
}
