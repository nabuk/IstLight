using System;
using System.Collections.Generic;

namespace ScriptingWrapper
{
    /// <summary>
    /// Creates script engine wrappers
    /// </summary>
    public static class ScriptEngineFactory
    {
        /// <summary>
        /// Creates script engine according to passed language argument.
        /// </summary>
        /// <param name="language">Script engine type to create.</param>
        /// <returns>Script engine wrapper.</returns>
        public static ScriptEngineBase CreateEngine(ScriptingLanguage language)
        {
            return languageCreatorMap[language]();
        }
        /// <summary>
        /// Creates script engine according to passed script language extension (i.e. 'py' for python)
        /// </summary>
        /// <param name="scriptExtension">Script language extension.</param>
        /// <returns>Script engine wrapper.</returns>
        /// <exception cref="ArgumentNullException">Thrown if scriptExtension argument is null.</exception>
        public static ScriptEngineBase TryCreateEngine(string scriptExtension)
        {
            if (scriptExtension == null) throw new ArgumentNullException("scriptExtension");

            string ext = scriptExtension.Replace(".", "").ToLower();
            if (!extensionToLanguageMap.ContainsKey(ext))
                return null;
            else
                return CreateEngine(extensionToLanguageMap[ext]);
        }
        
        

        private static Dictionary<ScriptingLanguage, Func<ScriptEngineBase>> languageCreatorMap =
            new Dictionary<ScriptingLanguage, Func<ScriptEngineBase>>
            {
                { ScriptingLanguage.IronPython,  () => new Implementations.PythonScriptEngine() }


            };

        private static Dictionary<string, ScriptingLanguage> extensionToLanguageMap =
            new Dictionary<string, ScriptingLanguage>
            {
                { "py", ScriptingLanguage.IronPython }
            };

        public static IEnumerable<KeyValuePair<string, ScriptingLanguage>> ExtensionToLanguageMap
        {
            get
            {
                return extensionToLanguageMap;
            }
        }

        /// <summary>
        /// Supported scipt language extensions.
        /// </summary>
        public static IEnumerable<string> AllowedExtensions { get { return extensionToLanguageMap.Keys; } }
    }
}
