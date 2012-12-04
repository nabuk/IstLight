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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScriptingWrapper;

namespace IstLight
{
    public class ScriptsFromDirectory : IScriptLoadService
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
                    Extension = Path.GetExtension(filePath).Replace(".",""),
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