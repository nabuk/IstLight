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

using System.Linq;

namespace IstLight.Services
{
    public abstract class ScriptAsyncLoadService<T> : IAsyncLoadService<T>
        where T : INamedItem
    {
        private readonly IScriptLoadService scripts;

        protected abstract ValueOrError<T> CreateInstance(Script script);

        public ScriptAsyncLoadService(IScriptLoadService scripts)
        {
            this.scripts = scripts;
        }

        public IReadOnlyList<IAsyncResult<T>> Load()
        {
            return scripts.Load().Select(script => new AsyncResultFromSyncJob<T>(() => CreateInstance(script))).AsReadOnlyList();
        }
    }
}
