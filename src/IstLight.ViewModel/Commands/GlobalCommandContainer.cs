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
using System.Linq;

namespace IstLight.Commands
{
    public class GlobalCommandContainer
    {
        private readonly Dictionary<string, IGlobalCommand> commandsDict;

        public GlobalCommandContainer(IEnumerable<IGlobalCommand> commands)
        {
            this.commandsDict = commands.ToDictionary(k => k.Key, v => v);
        }

        public IGlobalCommand this[string key]
        {
            get
            {
                return commandsDict[key];
            }
        }
    }
}
