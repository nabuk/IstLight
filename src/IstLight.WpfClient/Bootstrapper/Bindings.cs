using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Services;
using IstLight.Services.Decorators;
using Ninject.Modules;
using Ninject;

namespace IstLight.Bootstrapper
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ErrorListViewModel>().ToSelf().InSingletonScope();
            Kernel.Bind<IErrorReporter>().ToMethod(x => x.Kernel.Get<ErrorListViewModel>()).InSingletonScope();

            Kernel.Bind<Action<TickerViewModel>>().ToMethod(x => t => System.Windows.MessageBox.Show("Loading :" + t.Name));

            Kernel.Bind<IAsyncLoadService<ITickerProvider>>()
                .To<TickerProviderService>()
                .WhenInjectedInto<AsyncLoadServiceErrorDecorator<ITickerProvider>>()
                .InSingletonScope();

            Kernel.Bind<IScriptLoadService>()
                .To<ScriptsFromDirectory>()
                .WhenInjectedInto<TickerProviderService>()
                .WithConstructorArgument("path", "scripts\\providers");
            Kernel.Bind<IAsyncLoadService<ITickerProvider>>()
                .To<AsyncLoadServiceErrorDecorator<ITickerProvider>>()
                .InSingletonScope();
        }
    }
}
