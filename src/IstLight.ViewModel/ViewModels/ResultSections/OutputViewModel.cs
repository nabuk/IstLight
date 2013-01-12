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
using System.Linq;
using System.Text;
using IstLight.Simulation;

namespace IstLight.ViewModels.ResultSections
{
    public class OutputViewModel : ISectionViewModel
    {
        public OutputViewModel(SimulationResult result)
        {
            this.Rows = result.Select((q, i) => new OutputRowViewModel(q.Output, i, q.Date)).Where(r => r.Text != null);
        }

        public string Header
        {
            get { return "Output"; }
        }

        public IEnumerable<OutputRowViewModel> Rows { get; private set; }
    }
}
