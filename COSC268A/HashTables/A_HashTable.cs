using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTables;

public abstract class A_HashTable<K, V> : I_HashTable<K, V> where K : IComparable<K>
{
    // The table array
    // In the case of chaining this will be an array of data structures
    // With open addressing, it'll store the key-value pairs directly
    protected object[] oDataArray;

    // We should probably also keep track of how many items are in the hashtable:
    protected int iCount;

    // Every hashtable will have a load factor limit -
    // the maximum percent full that we will allow our array to get
    // before rehashing. This might vary from implemention to implemention.
    // Microsoft uses 0.72 for their hashtables, so we'll use that as our default
    protected double dLoadFactorLimit = 0.72;

    // Let's keep track of the number of collisions for analysis purposes
    protected int iNumCollisions = 0;

    // Some of those fields we'll want to expose as properties:
    public int Count {  get { return iCount; } }
    public int NumCollisions {  get { return iNumCollisions; } }

    // We'll also have a property for the current table size:
    public int HTSize {  get {  return oDataArray.Length; } }

    public abstract K[] Keys { get; }

    public abstract void Add(K key, V value);
    public abstract void Clear();
    public abstract IEnumerator<V> GetEnumerator();
    public abstract V Get(K key);
    public abstract V Remove(K key);

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // Every table is going to need to have a hash function
    // We'll provide a default hash function
    // This is going to be a two part hash function - the key is
    // going to be responsible for returning an integer,
    // and we'll just handle mapping that int to the space of our table indexes:
    protected int HashFunction(K key)
    {
        int hashcode = key.GetHashCode();
        // The hashcode could be any integer, so it could be negative
        // So we'll take the absolute value:
        return Math.Abs(hashcode % HTSize);
    }
}
