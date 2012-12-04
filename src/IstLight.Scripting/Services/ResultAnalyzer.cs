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

using System.Collections.Generic;
using IstLight.Simulation;

namespace IstLight.Services
{
    public class ResultAnalyzer : ScriptNamedItemBase, IResultAnalyzer
    {
        public ResultAnalyzer(string category, ParallelScriptExecutor executor) : base(executor)
        {
            this.Category = category;
        }

        public string Category
        {
            get; private set;
        }

        public IAsyncResult<IReadOnlyList<KeyValuePair<string, string>>> Analyze(SimulationResult result)
        {
            return executor.SafeExecuteAsync<IReadOnlyList<KeyValuePair<string, string>>>(engine =>
                ((KeyValuePair<string,string>[])engine.GetVariable("Analyze")(result)).AsReadOnlyList());
        }
    }
}
