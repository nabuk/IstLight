using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Services;
using Ninject.Modules;
using Ninject;

namespace IstLight.Bootstrapper
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ErrorListViewModel>().ToSelf().InSingletonScope();
            Kernel.Bind<IErrorReporter>().ToMethod(x => x.Kernel.Get<ErrorListViewModel>());

            Kernel.Bind<IAsyncLoadService<ITickerProvider>>().To<TickerProviderService>();
            Kernel.Bind<IAsyncLoadService<ITickerConverter>>().To<TickerConverterService>();
            Kernel.Bind<IAsyncLoadService<ITickerTransformer>>().To<TickerTransformerService>();
            Kernel.Bind<IAsyncLoadService<IResultAnalyzer>>().To<ResultAnalyzerService>();

            Kernel.Bind<IScriptLoadService>().To<ScriptsFromDirectory>()
                .WhenInjectedInto<TickerProviderService>().WithConstructorArgument("path", "scripts\\providers");
            Kernel.Bind<IScriptLoadService>().To<ScriptsFromDirectory>()
                .WhenInjectedInto<TickerConverterService>().WithConstructorArgument("path", "scripts\\converters");
            Kernel.Bind<IScriptLoadService>().To<ScriptsFromDirectory>()
                .WhenInjectedInto<TickerTransformerService>().WithConstructorArgument("path", "scripts\\transformers");
            Kernel.Bind<IScriptLoadService>().To<ScriptsFromDirectory>()
                .WhenInjectedInto<ResultAnalyzerService>().WithConstructorArgument("path", "scripts\\analyzers");


            Kernel.Bind<IAsyncLoadValidService<ITickerProvider>>().To<AsyncLoadValidService<ITickerProvider>>();

            Kernel.Bind<Action<TickerViewModel>>().ToMethod(x => t => System.Windows.MessageBox.Show("Loading :" + t.Name));
        }
    }
}
