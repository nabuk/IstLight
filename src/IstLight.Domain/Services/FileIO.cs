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

namespace IstLight.Services
{
    public class FileIO : IFileIO
    {
        #region IFileIO
        public RawFile Read(string filePath)
        {
            return new RawFile(
                Path.GetFileNameWithoutExtension(filePath),
                Path.GetExtension(filePath),
                File.ReadAllBytes(filePath));
        }

        public void Save(string path, RawFile file)
        {
            File.WriteAllBytes(Path.Combine(path, file.Name + "." + file.Format), file.Data);
        }
        #endregion //IFileIO
    }
}
