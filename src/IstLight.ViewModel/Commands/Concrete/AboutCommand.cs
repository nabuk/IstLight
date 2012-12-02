using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IstLight.Commands.Concrete
{
    public class AboutCommand : GlobalCommandBase
    {
        public AboutCommand()
            : base("About", new DelegateCommand(() => Process.Start("https://github.com/nabuk/IstLight#readme"))) { }
    }
}
