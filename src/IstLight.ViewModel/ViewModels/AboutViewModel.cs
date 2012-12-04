using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.ViewModels
{
    public class AboutViewModel
    {
        public AboutViewModel(string appName, string appVersion, string appCopyright)
	    {
            this.AppName = appName;
            this.AppVersion = "version: "+appVersion;
            this.AppCopyright = appCopyright;
	    }

        public string AppName { get; private set; }
        public string AppVersion { get; private set; }
        public string AppCopyright { get; private set; }
    }
}
