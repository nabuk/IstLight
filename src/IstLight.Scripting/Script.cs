using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight;
using IstLight.Extensions;
using ScriptingWrapper;

namespace IstLight.Scripting
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
