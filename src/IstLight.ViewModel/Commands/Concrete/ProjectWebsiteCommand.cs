using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IstLight.Commands.Concrete
{
    public class ProjectWebsiteCommand : GlobalCommandBase
    {
        public ProjectWebsiteCommand()
            : base("ProjectWebsite",
                new DelegateCommand(() => Process.Start("https://github.com/nabuk/IstLight#readme"))) { }
    }
}