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

        #region IContext
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

        public dynamic Invoke(string name, bool returnsValue, params dynamic[] args)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            dynamic m = this[name];
            if (returnsValue)
            {
                switch (args.Length)
                {
                    case 0: return m();
                    case 1: return m(args[0]);
                    case 2: return m(args[0], args[1]);
                    case 3: return m(args[0], args[1], args[2]);
                    case 4: return m(args[0], args[1], args[2], args[3]);
                    case 5: return m(args[0], args[1], args[2], args[3], args[4]);
                    case 6: return m(args[0], args[1], args[2], args[3], args[4], args[5]);
                    case 7: return m(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                    case 8: return m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                    case 9: return m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                    case 10: return m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                    case 11: return m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                    case 12: return m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                    case 13: return m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                    case 14: return m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                    case 15: return m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                    case 16: return m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]);
                }
            }
            else
            {
                switch (args.Length)
                {
                    case 0: m(); return null;
                    case 1: m(args[0]); return null;
                    case 2: m(args[0], args[1]); return null;
                    case 3: m(args[0], args[1], args[2]); return null;
                    case 4: m(args[0], args[1], args[2], args[3]); return null;
                    case 5: m(args[0], args[1], args[2], args[3], args[4]); return null;
                    case 6: m(args[0], args[1], args[2], args[3], args[4], args[5]); return null;
                    case 7: m(args[0], args[1], args[2], args[3], args[4], args[5], args[6]); return null;
                    case 8: m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]); return null;
                    case 9: m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]); return null;
                    case 10: m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]); return null;
                    case 11: m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]); return null;
                    case 12: m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]); return null;
                    case 13: m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]); return null;
                    case 14: m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]); return null;
                    case 15: m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]); return null;
                    case 16: m(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]); return null;
                }
            }

            throw new InvalidOperationException("Cannot execute method with more than 16 arguments");
        }

        public IEnumerable<KeyValuePair<string, dynamic>> GetItems()
        {
            return variables;
        }

        public dynamic Get(string name)
        {
            return Exists(name) ? this[name] : null;
        }

        public void Set(string name, dynamic value)
        {
            this[name] = value;
        }

        #endregion //IContext


        
    }
}
