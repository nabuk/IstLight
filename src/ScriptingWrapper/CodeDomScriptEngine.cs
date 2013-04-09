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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ScriptingWrapper.Helpers;

namespace ScriptingWrapper
{
    public abstract class CodeDomScriptEngine : ScriptEngineBase
    {
        private readonly CodeDomProvider provider;

        private Dictionary<string, dynamic> items = new Dictionary<string, dynamic>();
        

        private readonly StringBuilder sbOutput = new StringBuilder();

        private Type scriptType;

        
        private void AddAssembliesToProviderParameters(IEnumerable<string> libPaths, CompilerParameters providerParameters)
        {
            var paths = libPaths.ToArray();
            if (paths.Length == 0)
                return;

            providerParameters.ReferencedAssemblies.AddRange(paths);
        }

        public CodeDomScriptEngine(CodeDomProvider provider)
        {
            this.provider = provider;
        }

        public override dynamic GetVariable(string name)
        {
            try
            {
                return items[name];
            }
            catch(KeyNotFoundException)
            {
                throw new MissingMemberException(string.Format("Variable \"{0}\" does not exist.", name));
            }
        }

        public override T GetVariable<T>(string name)
        {
            return (T)GetVariable(name);
        }

        public override bool TryGetVariable<T>(string name, out T value)
        {
            dynamic dValue;
            bool result = items.TryGetValue(name, out dValue);
            value = result ? (T)dValue : default(T);
            return result;
        }

        public override bool TryGetVariable(string name, out dynamic value)
        {
            return items.TryGetValue(name, out value);
        }

        public override void SetVariable(string name, object value)
        {
            if (this.ContainsVariable(name))
                items[name] = value;
            else
                items.Add(name, value);
        }

        public override bool ContainsVariable(string name)
        {
            return items.ContainsKey(name);
        }

        public override IEnumerable<KeyValuePair<string, dynamic>> GetItems()
        {
            return items;
        }

        public override bool RemoveVariable(string name)
        {
            return items.Remove(name);
        }

        public override void ClearScope()
        {
            items.Clear();
        }

        public override bool SetScript(string script)
        {
            if (script == null) throw new ArgumentNullException("script");

            var providerParameters = new CompilerParameters();
            providerParameters.GenerateInMemory = false;
            AddAssembliesToProviderParameters(GetAssembliesFromSearchPaths(searchPaths), providerParameters);

            var compilationResult = provider.CompileAssemblyFromSource(providerParameters,new[] { script });
            var errors = compilationResult.Errors.Cast<CompilerError>().Where(x => !x.IsWarning).ToArray();
            if (errors.Length > 0)
            {
                this.scriptType = null;
                LastError =
                    errors
                    .Select(x => string.Format("{0} (Line {1}): {2}",
                        x.ErrorNumber, x.Line, x.ErrorText))
                    .Aggregate((s1, s2) => s1 + Environment.NewLine + s2);
                return false;
            }

            var compiledAssembly = compilationResult.CompiledAssembly;
            
            this.scriptType =
                compilationResult.CompiledAssembly.GetTypes()
                .Where(x => x.IsClass && x.IsPublic && x.BaseType == typeof(BaseScript))
                .SingleOrDefault();
            if (scriptType == null)
            {
                LastError =
                    string.Format("There must be exactly one public class derived from {0}.",
                        typeof(BaseScript).Name);
                return false;
            }
            return true;
        }

        public override bool IsScriptSet
        {
            get { return scriptType != null; }
        }

        protected override void OnExecute()
        {
            BaseScript script = (BaseScript)Activator.CreateInstance(scriptType);
            script.ctx = new Context(items);

            var orgOut = Console.Out;
            Console.SetOut(new StringWriter(sbOutput));

            script.Run();

            Console.SetOut(orgOut);

            items = ((Context)script.ctx).variables;
        }

        public override string Output
        {
            get { return sbOutput.ToString(); }
        }

        public override void ClearOutput()
        {
            sbOutput.Clear();
        }
    }
}
