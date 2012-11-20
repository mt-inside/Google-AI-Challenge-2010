using System.Collections;
using System.Collections.Generic;

namespace mtBot
{
    class Set<T> : IEnumerable<T>
    {
        private readonly IDictionary<T, bool> m_Dict = new Dictionary<T, bool>();

        public void Add( T item )
        {
            m_Dict.Add( item, true );
        }

        public void Remove( T item )
        {
            m_Dict.Remove( item );
        }

        /* Because this class implements IEnumerable, it magically gains Contains(T) & Contains(T,IEqualityComparer<T>) as extension methods from LINQ. */

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_Dict.Keys.GetEnumerator();
        }
    }
}
