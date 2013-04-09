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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace ScriptingWrapper
{
    public abstract class MsHostingScriptEngine : ScriptEngineBase
    {
        /// <summary>
        /// Wrapped engine.
        /// </summary>
        protected ScriptEngine engine;
        /// <summary>
        /// Script engine scope.
        /// </summary>
        protected ScriptScope scope;
        /// <summary>
        /// Compiled script.
        /// </summary>
        protected CompiledCode compiledScript;

        /// <summary>
        /// Execution output stream.
        /// </summary>
        private MemoryStream outputStream = new MemoryStream();

        protected override string FormatException(Exception ex)
        {
            ExceptionOperations eo = engine.GetService<ExceptionOperations>();
            return eo.FormatException(ex);
        }

        /// <summary>
        /// Script engine wrapper base class constructor.
        /// </summary>
        /// <param name="engine">Script engine.</param>
        /// <exception cref="ArgumentNullException">Thrown if engine argument is null.</exception>
        public MsHostingScriptEngine(ScriptEngine engine)
        {
            if (engine == null)
                throw new ArgumentNullException("engine");
            this.engine = engine;
            engine.Runtime.IO.SetOutput(outputStream = new MemoryStream(), Encoding.UTF8);
            scope = this.engine.CreateScope();
        }

        public override void SetVariable(string name, object value)
        {
            scope.SetVariable(name, value);
        }

        public override dynamic GetVariable(string name)
        {
            return scope.GetVariable(name);
        }

        public override T GetVariable<T>(string name)
        {
            return scope.GetVariable<T>(name);
        }

        public override bool TryGetVariable(string name, out dynamic value)
        {
            return scope.TryGetVariable(name, out value);
        }

        public override bool TryGetVariable<T>(string name, out T value)
        {
            return scope.TryGetVariable<T>(name, out value);
        }

        public override bool ContainsVariable(string name)
        {
            return scope.ContainsVariable(name);
        }

        public override bool RemoveVariable(string name)
        {
            return scope.RemoveVariable(name);
        }

        public override IEnumerable<KeyValuePair<string, dynamic>> GetItems()
        {
            return scope.GetItems();
        }

        public override bool SetScript(string script)
        {
            if (script == null) throw new ArgumentNullException("script");

            var currentSearchPaths = engine.GetSearchPaths();
            foreach (var path in base.GetSearchPaths())
                if(!currentSearchPaths.Contains(path))
                    currentSearchPaths.Add(path);
            engine.SetSearchPaths(currentSearchPaths);

            ScriptSource scriptSource;
            try
            {
                scriptSource = engine.CreateScriptSourceFromString(script, Microsoft.Scripting.SourceCodeKind.Statements);
            }
            catch (Exception ex)
            {
                this.LastError = string.Format("Exception has been thrown during script creation. Message: {0}", ex.Message);
                this.compiledScript = null;
                return false;
            }

            var errors = new CustomErrorListener();
            var compiled = scriptSource.Compile(errors);
            if (errors.Count > 0)
            {
                this.compiledScript = null;
                LastError = errors.ErrorList.Aggregate((s1, s2) => s1 + Environment.NewLine + s2);
                return false;
            }
            else
            {
                this.compiledScript = compiled;
                return true;
            }
        }

        public override bool IsScriptSet
        {
            get
            {
                return compiledScript != null;
            }
        }

        protected override void OnExecute()
        {
            compiledScript.Execute(scope);
        }

        public override void ClearScope()
        {
            this.scope = engine.CreateScope();
        }

        public override string Output
        {
            get
            {
                outputStream.Position = 0;
                return new StreamReader(outputStream).ReadToEnd().TrimEnd();
            }
        }

        public override void ClearOutput()
        {
            outputStream.Position = 0;
            outputStream.SetLength(0);
        }

        public override void Dispose()
        {
            outputStream.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Error output execution listener.
        /// </summary>
        private class CustomErrorListener : ErrorListener
        {
            private List<String> _errorList = new List<string>();
            public IEnumerable<string> ErrorList { get { return _errorList; } }

            public override void ErrorReported(ScriptSource source, string message, SourceSpan span, int errorCode, Severity severity)
            {
                _errorList.Add(string.Format("{0}; Line: {1}, Column: {2}", message, span.Start.Line, span.Start.Column));
            }

            public int Count
            {
                get { return _errorList.Count; }
            }
        }
    }
}
