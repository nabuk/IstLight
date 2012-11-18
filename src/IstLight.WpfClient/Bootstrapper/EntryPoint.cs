using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Bootstrapper
{
    public class EntryPoint
    {
        private readonly MainWindowAdapter mainWindow;
        private readonly MainViewModel mainViewModel;

        public EntryPoint(MainWindowAdapter mainWindow, MainViewModel mainViewModel)
        {
            this.mainWindow = mainWindow;
            this.mainViewModel = mainViewModel;
        }

        public void Enter()
        {
            mainWindow.AcceptViewModel(mainViewModel);
            mainWindow.Show();
        }
    }
}
