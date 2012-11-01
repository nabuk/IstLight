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
    /// <summary>
    /// Script engine wrapper base class.
    /// </summary>
    public abstract class ScriptEngineBase : IDisposable
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

        private void SaveExceptionAsLastError(Exception ex)
        {
            ExceptionOperations eo = engine.GetService<ExceptionOperations>();
            LastError = eo.FormatException(ex);
        }

        /// <summary>
        /// Script engine wrapper base class constructor.
        /// </summary>
        /// <param name="engine">Script engine.</param>
        /// <exception cref="ArgumentNullException">Thrown if engine argument is null.</exception>
        public ScriptEngineBase(ScriptEngine engine)
        {
            if (engine == null)
                throw new ArgumentNullException("engine");
            this.engine = engine;
            engine.Runtime.IO.SetOutput(outputStream = new MemoryStream(), Encoding.UTF8);
            scope = this.engine.CreateScope();
        }

        /// <summary>
        /// Sets variable in script scope.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="value">Variable value.</param>
        /// <exception cref="ArgumentNullException">Thrown if name argument is null.</exception>
        public void SetVariable(string name, object value)
        {
            scope.SetVariable(name, value);
        }

        /// <summary>
        /// Gets a value stored in the scope under the given name.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <returns>Variable.</returns>
        /// <exception cref="MissingMemberException">The specified name is not defined in the scope.</exception>
        /// <exception cref="ArgumentNullException">name is a null reference.</exception>
        public dynamic GetVariable(string name)
        {
            return scope.GetVariable(name);
        }

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
        public T GetVariable<T>(string name)
        {
            return scope.GetVariable<T>(name);
        }

        /// <summary>
        /// Tries to get variable from script scope.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="value">Variable value.</param>
        /// <returns>A return value indicates whether operation succeeeded or failed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if name argument is null.</exception>
        public bool TryGetVariable(string name, out dynamic value)
        {
            return scope.TryGetVariable(name, out value);
        }
        /// <summary>
        /// Tries to get variable from script scope.
        /// </summary>
        /// <typeparam name="T">Variable type.</typeparam>
        /// <param name="name">Variable name.</param>
        /// <param name="value">Variable value.</param>
        /// <returns>A return value indicates whether operation succeeeded or failed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if name argument is null.</exception>
        public bool TryGetVariable<T>(string name, out T value)
        {
            return scope.TryGetVariable<T>(name, out value);
        }

        /// <summary>
        /// Determines if this context or any outer scope contains the defined name.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <returns>True if scope contains variable.</returns>
        /// <exception cref="ArgumentNullException">Thrown if name argument is null.</exception>
        public bool ContainsVariable(string name)
        {
            return scope.ContainsVariable(name);
        }

        /// <summary>
        /// Tries to remove variable from script scope.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <returns>A return value indicates whether operation succeeeded or failed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if name argument is null.</exception>
        public bool RemoveVariable(string name)
        {
            return scope.RemoveVariable(name);
        }

        /// <summary>
        /// Gets an array of variable names and their values stored in the scope.
        /// </summary>
        public IEnumerable<KeyValuePair<string,dynamic>> GetItems()
        {
            return scope.GetItems();
        }

        /// <summary>
        /// Adds the search paths used by the engine.
        /// </summary>
        /// <param name="paths">Additional search paths.</param>
        /// <exception cref="NotSupportedException">The language doesn't allow to set search paths.</exception>
        public void AddSearchPaths(params string[] paths)
        {
            var currentSearchPath = engine.GetSearchPaths();
            foreach(var path in paths)
                currentSearchPath.Add(path);
            engine.SetSearchPaths(currentSearchPath);
        }

        /// <summary>
        /// Gets the search paths used by the engine.
        /// </summary>
        /// <returns>Search paths.</returns>
        public IEnumerable<string> GetSearchPaths()
        {
            return engine.GetSearchPaths();
        }

        /// <summary>
        /// Sets and compiles script. If script cannot be set or compiled, LastError property will contain explanation.
        /// </summary>
        /// <param name="script">Script content.</param>
        /// <returns>A return value indicates whether operation succeeeded or failed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if script argument is null.</exception>
        public bool SetScript(string script)
        {
            if (script == null) throw new ArgumentNullException("script");

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
                LastError = errors.ErrorList.Aggregate((s1,s2) => s1 + Environment.NewLine + s2);
                return false;
            }
            else
            {
                this.compiledScript = compiled;
                return true;
            }
        }

        /// <summary>
        /// Property indicating whether the script was set.
        /// </summary>
        public bool IsScriptSet
        {
            get
            {
                return compiledScript != null;
            }
        }

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
                compiledScript.Execute(scope);
                return true;
            }
            catch (ThreadAbortException tae)
            {
                if (tae.ExceptionState is Microsoft.Scripting.KeyboardInterruptException)
                {
                    Thread.ResetAbort();
                }

                SaveExceptionAsLastError(tae);
                return false;
            }
            catch (Exception ex)
            {
                SaveExceptionAsLastError(ex);
                return false;
            }
        }

        /// <summary>
        /// Clears variables.
        /// </summary>
        public void ClearScope()
        {
            this.scope = engine.CreateScope();
        }

        /// <summary>
        /// Underlying script language.
        /// </summary>
        public abstract ScriptingLanguage Language { get; }

        /// <summary>
        /// Script language name.
        /// </summary>
        /// <returns>Script language name.</returns>
        public override string ToString()
        {
            return Language.ToString();
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

        /// <summary>
        /// Last error(s) message(s) or null
        /// </summary>
        public string LastError
        {
            get; private set;
        }

        /// <summary>
        /// Execution output (if any).
        /// </summary>
        public string Output
        {
            get
            {
                outputStream.Position = 0;
                return new StreamReader(outputStream).ReadToEnd();
            }
        }

        /// <summary>
        /// Releases all reasources used by wrapper.
        /// </summary>
        public void Dispose()
        {
            outputStream.Dispose();
        }
    }
}
