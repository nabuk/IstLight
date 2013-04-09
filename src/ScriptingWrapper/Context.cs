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

namespace ScriptingWrapper
{
    class Context : IContext
    {
        public readonly Dictionary<string, dynamic> variables = new Dictionary<string, dynamic>();

        public Context() { }

        public Context(IEnumerable<KeyValuePair<string,dynamic>> variables)
        {
            foreach (var pair in variables)
                this[pair.Key] = pair.Value;
        }

        public dynamic this[string name]
        {
            get
            {
                return variables[name];
            }
            set
            {
                if (Exists(name))
                    variables[name] = value;
                else
                    variables.Add(name, value);
            }
        }

        public bool Exists(string name)
        {
            return variables.ContainsKey(name);
        }

        public IEnumerable<KeyValuePair<string, dynamic>> GetItems()
        {
            return variables;
        }
    }
}
