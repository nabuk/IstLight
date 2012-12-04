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
    public class EquityViewModel : ISectionViewModel
    {
        private readonly SimulationResult result;

        public EquityViewModel(SimulationResult result)
        {
            this.result = result;
            this.Rows = result.Select((x, i) => new EquityRowViewModel(result, i)).ToArray();
        }

        public IEnumerable<EquityRowViewModel> Rows { get; private set; }

        public DateTime From { get { return result.From; } }
        public DateTime To { get { return result.To; } }

        public IEnumerable<KeyValuePair<DateTime,double>> Points
        {
            get
            {
                return Rows.Select(x => new KeyValuePair<DateTime,double>(x.Date,x.Total));
            }
        }

        #region ISectionViewModel
        public string Header
        {
            get { return "Equity"; }
        }
        #endregion //ISectionViewModel
    }
}
