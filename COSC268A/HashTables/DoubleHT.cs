using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTables;

public class DoubleHT<K,V> : A_OpenAddressing<K, V> where K : IComparable<K>
{
    // Implement double hashing where the increment is
    // 1 + h(k) % (n -1)
    protected override int GetIncrement(int iAttempt, K key)
    {
        // We'll need to get the hashcode for the key:
        int hk = Math.Abs(key.GetHashCode());
        // The increment is going to be 1 + the hashcode modded by the table size minus 1
        int hprime = 1 + (hk % (HTSize - 1));
        // Return it multiplied by the number of attempts
        return iAttempt * hprime;
    }
}
