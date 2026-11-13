using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HashTables;

/// <summary>
/// A wrapper class for keys and values
/// </summary>
/// <typeparam name="K">The type of the key which determines where in
/// the hashtable the item will go.</typeparam>
/// <typeparam name="V">The type of the value</typeparam>
public class KeyValue<K, V> : IComparable<KeyValue<K, V>> where K: IComparable<K>
{

    // We'll store the key and the value:
    K kKey;
    V vValue;

    public KeyValue(K kKey, V vValue)
    {
        this.kKey = kKey;
        this.vValue = vValue;
    }

    public K Key { get { return kKey; } }
    public V Value { get { return vValue; } }

    // Keyvalue pairs are compared based on their Keys
    // The order of keyvalue pairs will be the order of the keys:
    public int CompareTo(KeyValue<K,V> other)
    {
        return this.Key.CompareTo(other.Key);
    }

    // We might also want to test if they're equal.
    // They're equal if their keys are equal:
    public override bool Equals(object obj)
    {
        KeyValue<K,V> otherKV = (KeyValue<K,V>)obj;
        return this.CompareTo(otherKV) == 0;
    }
}
