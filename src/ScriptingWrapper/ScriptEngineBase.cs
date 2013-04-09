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
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using ScriptingWrapper.Attributes;
using ScriptingWrapper.Helpers;

namespace ScriptingWrapper
{
    /// <summary>
    /// Script engine wrapper base class.
    /// </summary>
    public abstract class ScriptEngineBase : IDisposable
    {
        protected readonly List<string> searchPaths = new List<string>();

        protected IEnumerable<string> GetAssembliesFromSearchPaths(IEnumerable<string> searchPaths)
        {
            return searchPaths.SelectMany(path =>
                {
                    if (!Directory.Exists(path) && !File.Exists(path))
                        throw new DirectoryNotFoundException("Directory not found: " + path);

                    var attributes = File.GetAttributes(path);
                    if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        return Directory.GetFiles(path, "*.dll")
                            .Distinct()
                            .Where(libPath => CheckAssembly.IsManagedAssembly(libPath));
                    }
                    else
                    {
                        if (!CheckAssembly.IsManagedAssembly(path))
                            throw new FileNotFoundException("This is not a path to managed library: " + path);
                        return new[] { path };
                    }
                });
        }

        public ScriptEngineBase()
        {
            searchPaths.Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            //Microsoft.CSharp
            searchPaths.Add(typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException).Assembly.Location);
            //System.Core
            searchPaths.Add(typeof(System.Runtime.CompilerServices.CallSite).Assembly.Location);
        }

        /// <summary>
        /// Sets variable in script scope.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="value">Variable value.</param>
        /// <exception cref="ArgumentNullException">Thrown if name argument is null.</exception>
        public abstract void SetVariable(string name, object value);

        /// <summary>
        /// Gets a value stored in the scope under the given name.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <returns>Variable.</returns>
        /// <exception cref="MissingMemberException">The specified name is not defined in the scope.</exception>
        /// <exception cref="ArgumentNullException">name is a null reference.</exception>
        public abstract dynamic GetVariable(string name);

        /// <summary>
        /// Gets a value stored in the scope under the given name.  Converts the result
        ///     to the specified type using the conversion that the language associated with
        ///     the scope defines.  If no language is associated with the scope, the default
        ///     CLR conversion is attempted.
        /// </summary>
        /// <typeparam name="T">Variable type.</typeparam>
        /// <param name="name">Variable name.</param>
        /// <returns>Variable.</returns>
        /// <exception cref="MissingMemberException">The specified name is not defined in the scope.</exception>
        /// <exception cref="ArgumentNullException">name is a null reference.</exception>
        public abstract T GetVariable<T>(string name);

        /// <summary>
        /// Tries to get variable from script scope.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="value">Variable value.</param>
        /// <returns>A return value indicates whether operation succeeeded or failed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if name argument is null.</exception>
        public abstract bool TryGetVariable(string name, out dynamic value);

        /// <summary>
        /// Tries to get variable from script scope.
        /// </summary>
        /// <typeparam name="T">Variable type.</typeparam>
        /// <param name="name">Variable name.</param>
        /// <param name="value">Variable value.</param>
        /// <returns>A return value indicates whether operation succeeeded or failed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if name argument is null.</exception>
        public abstract bool TryGetVariable<T>(string name, out T value);

        /// <summary>
        /// Determines if this context or any outer scope contains the defined name.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <returns>True if scope contains variable.</returns>
        /// <exception cref="ArgumentNullException">Thrown if name argument is null.</exception>
        public abstract bool ContainsVariable(string name);

        /// <summary>
        /// Tries to remove variable from script scope.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <returns>A return value indicates whether operation succeeeded or failed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if name argument is null.</exception>
        public abstract bool RemoveVariable(string name);

        /// <summary>
        /// Gets an array of variable names and their values stored in the scope.
        /// </summary>
        public abstract IEnumerable<KeyValuePair<string, dynamic>> GetItems();

        /// <summary>
        /// Adds the search paths used by the engine.
        /// </summary>
        /// <param name="paths">Additional search paths.</param>
        public void AddSearchPaths(params string[] paths)
        {
            searchPaths.AddRange(paths);

        }

        /// <summary>
        /// Gets the search paths used by the engine.
        /// </summary>
        /// <returns>Search paths.</returns>
        public IEnumerable<string> GetSearchPaths()
        {
            return searchPaths;
        }

        /// <summary>
        /// Sets and compiles script. If script cannot be set or compiled, LastError property will contain explanation.
        /// </summary>
        /// <param name="script">Script content.</param>
        /// <returns>A return value indicates whether operation succeeeded or failed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if script argument is null.</exception>
        public abstract bool SetScript(string script);

        /// <summary>
        /// Property indicating whether the script was set.
        /// </summary>
        public abstract bool IsScriptSet { get; }

        /// <summary>
        /// Executes compiled script. If execution failed, LastError property will contain explanation.
        /// </summary>
        /// <returns>A value indicates whether the execution was successful or not.</returns>
        /// <exception cref="InvalidOperationException">Thrown if script was not set.</exception>
        public bool Execute()
        {
            if (!IsScriptSet)
                throw new InvalidOperationException("Script was not set.");

            try
            {
                OnExecute();
                return true;
            }
            catch (ThreadAbortException tae)
            {
                if (tae.ExceptionState is Microsoft.Scripting.KeyboardInterruptException)
                {
                    Thread.ResetAbort();
                }

                LastError = FormatException(tae);
                return false;
            }
            catch (Exception ex)
            {
                LastError = FormatException(ex);
                return false;
            }

        }

        protected abstract void OnExecute();

        protected virtual string FormatException(Exception ex)
        {
            string error = string.Empty;
            while (ex != null)
            {
                error += string.Format("{1}{0}Trace: {2}{0}",
                    Environment.NewLine,
                    ex.Message,
                    ex.StackTrace);
                ex = ex.InnerException;
            }
            return error;
        }

        /// <summary>
        /// Clears variables.
        /// </summary>
        public abstract void ClearScope();

        /// <summary>
        /// Underlying script language.
        /// </summary>
        public ScriptingLanguage Language
        {
            get
            {
                return ((LanguageAttribute)this.GetType()
                    .GetCustomAttributes(typeof(LanguageAttribute), true)
                    .First())
                    .Language;
            }
        }

        /// <summary>
        /// Script language name.
        /// </summary>
        /// <returns>Script language name.</returns>
        public override string ToString()
        {
            return Language.ToString();
        }


        /// <summary>
        /// Last error(s) message(s) or null
        /// </summary>
        public string LastError
        {
            get;
            protected set;
        }

        /// <summary>
        /// Execution output (if any).
        /// </summary>
        public abstract string Output { get; }

        public abstract void ClearOutput();

        public string SyntaxHighlightingRules { get; internal set; }

        /// <summary>
        /// Releases all reasources used by wrapper.
        /// </summary>
        public virtual void Dispose() { }
    }
}
