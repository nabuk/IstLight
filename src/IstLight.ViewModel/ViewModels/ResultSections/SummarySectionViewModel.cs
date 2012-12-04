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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using IstLight.Services;
using IstLight.Simulation;

namespace IstLight.ViewModels.ResultSections
{
    public class SummarySectionViewModel : ViewModelBase, ISectionViewModel
    {
        private readonly SimulationResult result;
        private readonly IReadOnlyList<IAsyncResult<IResultAnalyzer>> analyzers;

        public SummarySectionViewModel(IReadOnlyList<IAsyncResult<IResultAnalyzer>> analyzers, SimulationResult result)
        {
            this.analyzers = analyzers;
            this.result = result;

            this.Groups = new ReadOnlyCollection<SummaryGroupViewModel>(
                analyzers.Select(a => new SummaryGroupViewModel(a, result)).ToList());

        }

        #region ISectionViewModel
        public string Header
        {
            get { return "Summary"; }
        }
        #endregion //ISectionViewModel

        public ReadOnlyCollection<SummaryGroupViewModel> Groups { get; private set; }
    }
}