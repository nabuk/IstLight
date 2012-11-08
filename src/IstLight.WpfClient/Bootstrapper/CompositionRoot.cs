using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ninject;

namespace IstLight.Bootstrapper
{
    public class CompositionRoot : IDisposable
    {
        private IKernel kernel;

        private CompositionRoot()
        {
            this.kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
        }

        public static MainViewModel Compose()
        {
            using(var root = new CompositionRoot())
            {
                return root.kernel.Get<MainViewModel>();
            }
        }

        #region IDisposable
        public void Dispose()
        {
            this.kernel.Dispose();
        }
        #endregion //IDisposable
    }
}

