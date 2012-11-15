using IstLight.Services;
using IstLight.Services.Decorators;
using IstLight.Settings;
using Ninject;
using Ninject.Modules;

namespace IstLight.Bootstrapper
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ErrorListViewModel>().ToSelf().InSingletonScope();
            Kernel.Bind<IErrorReporter>().ToMethod(x => x.Kernel.Get<ErrorListViewModel>()).InSingletonScope();

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

            Kernel.Bind<ISimulationSettings>().To<SimulationSettings>();
            Kernel.Bind<SimulationSettingsViewModel>().ToSelf().InSingletonScope();

        }
    }
}
