// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

using System.Linq;
using System.Reflection;
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
using ScriptingWrapper;

namespace IstLight.Bootstrapper
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<StrategyTypes>().ToMethod(ctx =>
                {
                    var dictLangToExt = ScriptEngineFactory.ExtensionToLanguageMap.ToDictionary(x => x.Value, x => x.Key);
                    return new StrategyTypes(
                        ScriptEngineFactory.ExtensionToLanguageMap.ToDictionary(x => x.Key, x => x.Value.GetDescription()),
                        ScriptEngineFactory.LanguageSyntaxHighlighting.ToDictionary(x => dictLangToExt[x.Key], x => x.Value),
                        ScriptEngineFactory.LanguageExampleScript.ToDictionary(x => dictLangToExt[x.Key], x => x.Value)
                        );
                }).InSingletonScope();
                
                    
            Kernel.Bind<TickerExplorerViewModel>().ToSelf().InSingletonScope();
            Kernel.Bind<StrategyExplorerViewModel>().ToSelf().InSingletonScope();
            Kernel.Bind<SimulationSettingsViewModel>().ToSelf().InSingletonScope();
            Kernel.Bind<ErrorListViewModel>().ToSelf().InSingletonScope();
            Kernel.Bind<TickerOpenerViewModel>().ToSelf().InSingletonScope();

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

            Kernel.Bind<IAsyncLoadService<ITickerConverter>>().To<TickerConverterService>()
                .WhenInjectedInto<AsyncLoadServiceErrorDecorator<ITickerConverter>>()
                .InSingletonScope();
            Kernel.Bind<IAsyncLoadService<ITickerConverter>>().To<AsyncLoadServiceErrorDecorator<ITickerConverter>>().InSingletonScope();
            Kernel.Bind<IScriptLoadService>().To<ScriptsFromDirectory>()
                .WhenInjectedInto<TickerConverterService>()
                .WithConstructorArgument("path", "scripts\\converters");

            Kernel.Bind<IScriptLoadService>().To<ScriptsFromDirectory>()
                .WhenInjectedInto<ScriptStrategyFactory>()
                .WithConstructorArgument("path", "scripts\\functions");

            Kernel.Bind<IFileIO>().To<FileIO>()
                .WhenInjectedExactlyInto<FileIOErrorDecorator>()
                .InSingletonScope();
            Kernel.Bind<IFileIO>().To<FileIOErrorDecorator>().InSingletonScope();

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

            Kernel.Bind<AboutViewModelFactory>().ToMethod(x =>
                new AboutViewModelFactory(Assembly.GetExecutingAssembly()))
                .InSingletonScope();
        }
    }
}
