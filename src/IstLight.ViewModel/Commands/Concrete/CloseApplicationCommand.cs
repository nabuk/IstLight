using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Commands.Concrete
{
    public class CloseApplicationCommand : GlobalCommandBase
    {
        public CloseApplicationCommand(IWindow mainWindow)
            : base("Close", new DelegateCommand(mainWindow.Close)) { }
    }
}
