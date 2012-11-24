using System.Linq;
using IstLight.Commands;
using IstLight.Services;
using IstLight.Services.Decorators;
using IstLight.Settings;
using IstLight.Simulation;
using IstLight.Strategy;
using IstLight.Synchronization;
using IstLight.ViewModels;
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

            Kernel.Bind<IAsyncLoadService<ITickerProvider>>().To<TickerProviderService>()
                .WhenInjectedInto<AsyncLoadServiceErrorDecorator<ITickerProvider>>()
                .InSingletonScope();
            Kernel.Bind<IAsyncLoadService<ITickerProvider>>().To<AsyncLoadServiceErrorDecorator<ITickerProvider>>().InSingletonScope();
            Kernel.Bind<IScriptLoadService>().To<ScriptsFromDirectory>()
                .WhenInjectedInto<TickerProviderService>()
                .WithConstructorArgument("path", "scripts\\providers");

            Kernel.Bind<IAsyncLoadService<IResultAnalyzer>>().To<ResultAnalyzerService>()
                .WhenInjectedInto<AsyncLoadServiceErrorDecorator<IResultAnalyzer>>()
                .InSingletonScope();
            Kernel.Bind<IAsyncLoadService<IResultAnalyzer>>().To<AsyncLoadServiceErrorDecorator<IResultAnalyzer>>().InSingletonScope();
            Kernel.Bind<IScriptLoadService>().To<ScriptsFromDirectory>()
                .WhenInjectedInto<ResultAnalyzerService>()
                .WithConstructorArgument("path", "scripts\\report");

            Kernel.Bind<IScriptLoadService>().To<ScriptsFromDirectory>()
                .WhenInjectedInto<ScriptStrategyFactory>()
                .WithConstructorArgument("path", "scripts\\functions");

            Kernel.Bind<IStrategyCreator>().To<StrategyCreator>().InSingletonScope();
            Kernel.Bind<ISyncTickersGetter>().To<SyncTickersGetter>().InSingletonScope();
            Kernel.Bind<ISimulationSettingsGetter>().To<SimulationSettingsGetter>().InSingletonScope();
            Kernel.Bind<ISimulationSettings>().To<SimulationSettings>();

            Kernel.Bind<SimulationInput>().ToSelf().InSingletonScope();

            Kernel.Bind<MainWindowAdapter>().ToSelf().InSingletonScope()
                .WithConstructorArgument("mainWindow", new MainWindow());

            Kernel.Bind<IWindow>().ToMethod(x => x.Kernel.Get<MainWindowAdapter>());

            Kernel.Bind<GlobalCommandContainer>().ToSelf().InSingletonScope();
            foreach (var t in typeof(IGlobalCommand).Assembly.GetTypes().Where(t => typeof(IGlobalCommand).IsAssignableFrom(t) && !t.IsAbstract))
                Kernel.Bind<IGlobalCommand>().To(t);

            Kernel.Bind<ISimulationRunner>().To<SimulationRunnerResultDecorator>().InSingletonScope();
            Kernel.Bind<ISimulationRunner>().To<SimulationRunnerProgressDecorator>()
                .WhenInjectedInto<SimulationRunnerResultDecorator>()
                .InSingletonScope();
            Kernel.Bind<ISimulationRunner>().To<SimulationRunnerErrorDecorator>()
                .WhenInjectedInto<SimulationRunnerProgressDecorator>()
                .InSingletonScope();
            Kernel.Bind<ISimulationRunner>().To<SimulationRunner>()
                .WhenInjectedInto<SimulationRunnerErrorDecorator>()
                .InSingletonScope();
        }
    }
}
