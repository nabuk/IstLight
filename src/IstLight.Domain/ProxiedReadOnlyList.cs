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

namespace IstLight
{
    public class ProxiedReadOnlyList<T> : IReadOnlyList<T>, IEnumerable<T>
    {
        public ProxiedReadOnlyList(ProxiedIndexer<T> indexer, Func<int> countGetter)
        {
            if (indexer == null) throw new ArgumentNullException("indexer");
            if (countGetter == null) throw new ArgumentNullException("countGetter");

            this.indexer = indexer;
            this.countDelegate = countGetter;
        }

        #region IReadOnlyList

        /// <summary>
        /// Override to decorate in fly.
        /// </summary>
        public virtual T this[int index]
        {
            get
            {
                if (index < 0 || index >= countDelegate())
                    throw new IndexOutOfRangeException();

                return indexer(index);
            }
        }

        public int Count
        {
            get
            {
                int count = countDelegate();
                return count < 0 ? 0 : count;
            }
        }

        private readonly ProxiedIndexer<T> indexer;
        private readonly Func<int> countDelegate;
        #endregion //IReadOnlyList

        #region IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return this[i];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion //IEnumerable
    }
}
