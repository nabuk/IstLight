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
            var assemblyName = executingAssembly.GetName();
            string appName = assemblyName.Name;
            string appVersion = assemblyName.Version.ToString();
            string appCopyright = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(
                executingAssembly, typeof(AssemblyCopyrightAttribute), false)).Copyright;
            
            this.about = new AboutViewModel(appName, appVersion, appCopyright);
        }

        public AboutViewModel About { get { return this.about; } }
    }
}
