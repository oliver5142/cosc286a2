using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTables;

public class QuadraticHT<K, V>: A_OpenAddressing<K,V> where K: IComparable<K>
{
    // I'll set our values for c:
    double c1 = 0.5;
    double c2 = 0.5;


    public QuadraticHT()
    {
        // The load factor limit of an open addressing hashtable using
        // a quadratic probe seqeucne should never be higher than 0.5:
        dLoadFactorLimit = 0.5;
    }

    // Override GetIncrement to use our quadratic formula:
    protected override int GetIncrement(int iAttempt, K key)
    {
        int increment = (int)(c1 * iAttempt + c2 * Math.Pow(iAttempt, 2));
        return increment;
    }
}
