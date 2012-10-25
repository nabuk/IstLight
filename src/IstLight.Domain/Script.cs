using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Extensions;

namespace IstLight.Domain
{
    public class Script
    {
        protected Script(string extension, string language)
        {
            this.Extension = extension;
            this.Language = language;
        }

        public string Name { get; set; }
        public string Content { get; set; }

        public string Extension { get; private set; }
        public string Language { get; private set; }

        public static Script CreateByLanguage(string language)
        {
            if (language == null) throw new ArgumentNullException("language");

            return ExtensionToLanguageMap
                .Where(x => string.Equals(x.Value, language, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => new Script(x.Key, x.Value))
                .FirstOrDefault();
        }

        public static Script CreateByExtension(string extension)
        {
            if (extension == null) throw new ArgumentNullException("extension");

            return ExtensionToLanguageMap
                .Where(x => string.Equals(x.Key, extension, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => new Script(x.Key, x.Value))
                .FirstOrDefault();
        }

        public static IReadOnlyCollection<KeyValuePair<string, string>> ExtensionToLanguageMap
        {
            get
            {
                return
                    Scripting.ScriptEngineFactory.ExtensionToLanguageMap
                    .Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value.ToString()))
                    .AsReadOnlyCollection();
            }
        }
    }
}
