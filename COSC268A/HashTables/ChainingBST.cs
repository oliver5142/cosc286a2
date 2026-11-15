using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTables
{
    public class ChainingBST<K, V> : A_HashTable<K, V> where K : IComparable<K>
    {

        // Let's set an initial table size and the factor by which it will expand:
        private const int iExpansionFactor = 2;
        private const int iInitialSize = 5;


        public override K[] Keys => throw new NotImplementedException();

        public override void Add(K key, V value)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override V Get(K key)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<V> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override V Remove(K key)
        {
            throw new NotImplementedException();
        }
    }
}
