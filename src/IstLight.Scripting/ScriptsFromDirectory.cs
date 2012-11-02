using System.Collections.Generic;
using System.IO;
using System.Linq;
using IstLight.Extensions;
using ScriptingWrapper;

namespace IstLight
{
    public class ScriptsFromDirectory : IScriptService
    {
        private readonly string directoryPath;

        public ScriptsFromDirectory(string path)
        {
            this.directoryPath = path;            
        }

        public IReadOnlyList<Script> Load()
        {
            var allowedExtensions = new HashSet<string>(ScriptEngineFactory.AllowedExtensions);
            
            return Directory.GetFiles(directoryPath).Select(filePath =>
                new
                {
                    Name = Path.GetFileNameWithoutExtension(filePath),
                    Extension = Path.GetExtension(filePath),
                    Path = filePath
                })
                .Where(x => allowedExtensions.Contains(x.Extension))
                .AsParallel()
                .Select(x =>
                    {
                        try
                        {
                            using (var reader = new StreamReader(x.Path))
                                return new Script(x.Name, x.Extension, reader.ReadToEnd());
                        } catch (IOException) { return null; }
                    })
                .AsEnumerable()
                .Where(x => x != null)
                .AsReadOnlyList();
        }
    }
}