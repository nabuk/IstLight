using IstLight.Commands;
using IstLight.Commands.Concrete;
using IstLight.Services;
using IstLight.Services.Decorators;
using IstLight.Settings;
using IstLight.Strategy;
using IstLight.Synchronization;
using Ninject;
using Ninject.Modules;

namespace IstLight.Bootstrapper
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<TickerExplorerViewModel>().ToSelf().InSingletonScope();
            Kernel.Bind<StrategyExplorerViewModel>().ToSelf().InSingletonScope();
            Kernel.Bind<SimulationSettingsViewModel>().ToSelf().InSingletonScope();
            Kernel.Bind<ErrorListViewModel>().ToSelf().InSingletonScope();

            Kernel.Bind<IErrorReporter>().ToMethod(x => x.Kernel.Get<ErrorListViewModel>()).InSingletonScope();

            Kernel.Bind<IAsyncLoadService<ITickerProvider>>()
                .To<TickerProviderService>()
                .WhenInjectedInto<AsyncLoadServiceErrorDecorator<ITickerProvider>>()
                .InSingletonScope();
            Kernel.Bind<IAsyncLoadService<ITickerProvider>>()
                .To<AsyncLoadServiceErrorDecorator<ITickerProvider>>()
                .InSingletonScope();

            Kernel.Bind<IScriptLoadService>()
                .To<ScriptsFromDirectory>()
                .WhenInjectedInto<TickerProviderService>()
                .WithConstructorArgument("path", "scripts\\providers");

            Kernel.Bind<IScriptLoadService>()
                .To<ScriptsFromDirectory>()
                .WhenInjectedInto<ScriptStrategyFactory>()
                .WithConstructorArgument("path", "scripts\\functions");

            Kernel.Bind<IStrategyCreator>().To<StrategyCreator>();

            Kernel.Bind<ISyncTickersGetter>()
                .To<SyncTickersGetter>()
                .WhenInjectedInto<SyncTickersGetterErrorDecorator>()
                .InSingletonScope();
            Kernel.Bind<ISyncTickersGetter>()
                .To<SyncTickersGetterErrorDecorator>()
                .InSingletonScope();

            Kernel.Bind<ISimulationSettingsGetter>()
                .To<SimulationSettingsGetter>()
                .InSingletonScope();


            Kernel.Bind<ISimulationSettings>().To<SimulationSettings>();

            //zly window jest wstrzykiwany do entry point
            Kernel.Bind<MainWindowAdapter>().ToSelf()
                .InSingletonScope()
                .WithConstructorArgument("mainWindow", new MainWindow());

            Kernel.Bind<IWindow>().ToMethod(x => x.Kernel.Get<MainWindowAdapter>());
                

            Kernel.Bind<IGlobalCommand>().To<CloseApplicationCommand>();
            Kernel.Bind<GlobalCommandContainer>().ToSelf()
                .InSingletonScope();

        }
    }
}
