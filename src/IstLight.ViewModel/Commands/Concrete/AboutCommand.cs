using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IstLight.ViewModels;

namespace IstLight.Commands.Concrete
{
    public class AboutCommand : GlobalCommandBase
    {
        public AboutCommand(IWindow mainWindow, AboutViewModelFactory aboutFactory)
            : base("About", new DelegateCommand(() => mainWindow.CreateChild(aboutFactory.About).ShowDialog())) { }
    }
}
