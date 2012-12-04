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

namespace IstLight
{
    public class Script
    {
        public Script(string name, string extension, string content)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(extension)) throw new ArgumentNullException("extension");
            content = content ?? "";

            this.Name = name;
            this.Extension = extension;
            this.Content = content;
        }

        public string Name { get; private set; }
        public string Content { get; private set; }
        public string Extension { get; private set; }

        //public string Language { get; private set; }

        //public static Script CreateByLanguage(string language)
        //{
        //    if (language == null) throw new ArgumentNullException("language");

        //    return ExtensionToLanguageMap
        //        .Where(x => string.Equals(x.Value, language, StringComparison.InvariantCultureIgnoreCase))
        //        .Select(x => new Script(x.Key, x.Value))
        //        .FirstOrDefault();
        //}

        //public static Script CreateByExtension(string extension)
        //{
        //    if (extension == null) throw new ArgumentNullException("extension");

        //    return ExtensionToLanguageMap
        //        .Where(x => string.Equals(x.Key, extension, StringComparison.InvariantCultureIgnoreCase))
        //        .Select(x => new Script(x.Key, x.Value))
        //        .FirstOrDefault();
        //}

        //public static IReadOnlyList<KeyValuePair<string, string>> ExtensionToLanguageMap
        //{
        //    get
        //    {
        //        return
        //            ScriptEngineFactory.ExtensionToLanguageMap
        //            .Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value.ToString()))
        //            .AsReadOnlyList();
        //    }
        //}
    }
}
