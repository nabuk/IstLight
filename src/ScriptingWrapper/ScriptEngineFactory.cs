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
