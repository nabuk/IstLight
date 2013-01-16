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

using System;
using System.Reflection;
using GalaSoft.MvvmLight;
using IstLight.Commands;

namespace IstLight.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(
            TickerExplorerViewModel tickerExplorer,
            StrategyExplorerViewModel strategyExplorer,
            ErrorListViewModel errorList,
            SimulationSettingsViewModel simulationSettings,
            GlobalCommandContainer commands)
        {
            this.TickerExplorer = tickerExplorer;
            this.ErrorList = errorList;
            this.SimulationSettings = simulationSettings;
            this.StrategyExplorer = strategyExplorer;
            this.Commands = commands;

            var assembly = Assembly.GetEntryAssembly();
            var v = new Version(((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(
                assembly, typeof(AssemblyFileVersionAttribute), false)).Version);
            Title = string.Format("{0} v{1}.{2}",
                assembly.GetName().Name,
                v.Major,
                v.Minor);
        }

        public TickerExplorerViewModel TickerExplorer { get; private set; }
        public StrategyExplorerViewModel StrategyExplorer { get; private set; }
        public SimulationSettingsViewModel SimulationSettings { get; private set; }
        public ErrorListViewModel ErrorList { get; private set; }

        public GlobalCommandContainer Commands { get; private set; }

        public string Title { get; private set; }
    }
}
