using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain
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
