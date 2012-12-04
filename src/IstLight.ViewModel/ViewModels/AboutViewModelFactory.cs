using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IstLight.ViewModels
{
    public class AboutViewModelFactory
    {
        private readonly AboutViewModel about;

        public AboutViewModelFactory(Assembly executingAssembly)
        {
            string appName = executingAssembly.GetName().Name;
            string appVersion = ((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(
                executingAssembly, typeof(AssemblyFileVersionAttribute), false)).Version;
            string appCopyright = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(
                executingAssembly, typeof(AssemblyCopyrightAttribute), false)).Copyright;
            
            this.about = new AboutViewModel(appName, appVersion, appCopyright);
        }

        public AboutViewModel About { get { return this.about; } }
    }
}
