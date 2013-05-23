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
using ScriptingWrapper.Attributes;

namespace ScriptingWrapper.Implementations
{
    [Language(ScriptingLanguage.IronRuby)]
    [AllowedExtension("rb")]
    sealed class RubyScriptEngine : MsHostingScriptEngine
    {
        private Context ctx = new Context();

        public RubyScriptEngine() : base(IronRuby.Ruby.CreateEngine()) { }

        public override void SetVariable(string name, object value)
        {
            ctx[name] = value;
        }

        public override dynamic GetVariable(string name)
        {
            if(ContainsVariable(name))
                return ctx[name];
            else
                throw new MissingMemberException(string.Format("Variable \"{0}\" does not exist.", name));
        }

        public override T GetVariable<T>(string name)
        {
            return (T)GetVariable(name);
        }

        public override bool TryGetVariable(string name, out dynamic value)
        {
            if (ContainsVariable(name))
            {
                value = ctx[name];
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public override bool TryGetVariable<T>(string name, out T value)
        {
            if (ContainsVariable(name))
            {
                value = (T)ctx[name];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public override bool ContainsVariable(string name)
        {
            return ctx.Exists(name);
        }

        public override bool RemoveVariable(string name)
        {
            if (ContainsVariable(name))
            {
                ctx.variables.Remove(name);
                return true;
            }
            else
                return false;
        }

        public override IEnumerable<KeyValuePair<string, dynamic>> GetItems()
        {
            return ctx.variables;
        }

        protected override void OnExecute()
        {
            scope.SetVariable("ctx", (IContext)ctx);
            base.OnExecute();
        }

        public override void ClearScope()
        {
            ctx.variables.Clear();
            base.ClearScope();
        }
    }


}
