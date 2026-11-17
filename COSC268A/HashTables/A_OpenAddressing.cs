using System;
using System.Collections;
using System.Collections.Generic;

namespace HashTables;

public abstract class A_OpenAddressing<K, V> : A_HashTable<K, V> where K : IComparable<K>
{
    public override K[] Keys => throw new NotImplementedException();

    // Most of our open addressing hashtables will want to keep track 
    // of the next prime number:
    private PrimeNumber pn = new PrimeNumber();
    // Keep track of the number of tombstones:
    private int iTombstone = 0;

    protected A_OpenAddressing()
    {
        // Our initial array size will be the first prime number in our sequence:
        oDataArray = new object[pn.GetNextPrime()];
    }

    public override void Add(K key, V value)
    {
        // Keep track of how many attempts have been made:
        int iAttempt = 1;
        // Grab the initial hashtable location:
        int iInitialHashLocation = HashFunction(key);
        // Wrap up the key and value we're given into a KeyValue pair:
        KeyValue<K, V> kv = new KeyValue<K, V>(key, value);
        // I'm going to keep track of our current position in the hashtable
        //   which will start the same as the initial position:
        int iCurrentLocation = iInitialHashLocation;

        // Keep to checking in the hashtable according to our probe sequence
        // until we find a null (an empty slot) in our table:
        while (oDataArray[iCurrentLocation] != null)
        {
            // Check for duplicate key - if we find a dupe throw an exception:
            if (oDataArray[iCurrentLocation].GetType() == typeof(KeyValue<K, V>))
            {
                // I'll need to cast the object to KeyValue:
                KeyValue<K, V> kvInBucket = (KeyValue<K, V>)oDataArray[iCurrentLocation];
                // Check if the keys are equal - if they are, we've got a duplicate:
                if (kvInBucket.Equals(kv))
                {
                    throw new ApplicationException("Key " + key + " already exists in hashtable.");
                }
            }
            // We haven't found a dupe or a null, so we need to advance our current location
            // to look at the next bucket indicated by GetIncrement:
            iCurrentLocation = iInitialHashLocation + GetIncrement(iAttempt++, key);
            // If we fall off the bottom of the table, loop back to the top using mod
            iCurrentLocation %= HTSize;
            // This counts as a collision, so make sure to increase the collision count:
            iNumCollisions++;
        }

        // Once we've gone through our probe sequence and ended up on an empty
        // bucket, we just add the keyvalue to our current position:
        oDataArray[iCurrentLocation] = kv;
        // Remember to increment the count:
        iCount++;

        // Remember to check for overloading:
        if (isOverloaded())
        {
            expandHashTable();
        }
    }

    private bool isOverloaded()
    {
        return (((iCount + iTombstone) / (double)HTSize)) > dLoadFactorLimit;
    }

    private void expandHashTable()
    {
        // Get a reference to the old hashtable:
        object[] oOldDataArray = oDataArray;
        // Create a new bigger array:
        oDataArray = new object[pn.GetNextPrime()];

        // Reset our counters:
        iCount = 0;
        iNumCollisions = 0;

        foreach (object o in oOldDataArray)
        {
            if (o != null && o.GetType() == typeof(KeyValue<K, V>))
            {
                KeyValue<K, V> kv = (KeyValue<K, V>)o;
                Add(kv.Key, kv.Value);
            }
        }
    }

    public override void Clear()
    {
        pn = new PrimeNumber();
        oDataArray = new object[pn.GetNextPrime()];
        iCount = 0;
        iNumCollisions = 0;
        iTombstone = 0;
    }

    public override V Get(K key)
    {
        // Find the initial location in the table using HashFunction,
        // and then go through the table (advancing by an amount dependet on GetIncrement)
        // until either you find a KeyValue pair whose key matches what you're looking for
        // or you hit a null in which case throw an ApplicationException("Key not found")

        V vReturn = default(V);
        // Get the initial position to check:
        int iInitialHashLocation = HashFunction(key);
        // We'll keep track of our current position:
        int iCurrentLocation = iInitialHashLocation;
        // Keep track of our number of attempts:
        int iAttempts = 1;
        // Keep track of whether or not we've found it:
        bool bFound = false;
        // set intitial tombstone location out of bounds
        int tombstoneLocation = -1;

        // Keep looping until either we find the tiem or we hit a null:
        while (!bFound && oDataArray[iCurrentLocation] != null)
        {
            //check if the spot we are checking is a tombstone, if it is and we haven't recorded a tombtone location yet then we will store it
            if (oDataArray[iCurrentLocation].GetType() == typeof(Tombstone) && tombstoneLocation == -1)
            {
                tombstoneLocation = iCurrentLocation;
            }
            if (oDataArray[iCurrentLocation].GetType() == typeof(KeyValue<K, V>))
            {
                KeyValue<K, V> kv = (KeyValue<K, V>)oDataArray[iCurrentLocation];
                if (kv.Key.Equals(key))
                {
                    // If the keys match we've found the item:
                    vReturn = kv.Value;
                    // if we've recorded a tombstoneLocation than we will set that to be the kv we just found
                    if (tombstoneLocation != -1)
                    {
                        oDataArray[tombstoneLocation] = kv;
                        oDataArray[iCurrentLocation] = new Tombstone();
                    }

                    bFound = true;
                }
            }
            // Advance to the next position:
            iCurrentLocation = iInitialHashLocation + GetIncrement(iAttempts++, key);
            // Mod by the table size so we loop around the table:
            iCurrentLocation %= HTSize;
        }

        // If we don't find the key, throw an ApplicationException:
        if (!bFound)
        {
            throw new ApplicationException("Key " + key + " not found in table.");
        }


        return vReturn;
    }

    /// <summary>
    /// For a given attempt #, return the increment to add on to the
    /// initial table position
    /// </summary>
    /// <param name="iAttempt">The number of attempts, starting at 0</param>
    /// <param name="key">They key</param>
    /// <returns>The amount to add to the initial position</returns>
    abstract protected int GetIncrement(int iAttempt, K key);

    public override IEnumerator<V> GetEnumerator()
    {
        return new OpenAddressEnumerator(this);
    }

    public override V Remove(K key)
    {
        V vReturn = default(V);
        // Get the initial position to check:
        int iInitialHashLocation = HashFunction(key);
        // We'll keep track of our current position:
        int iCurrentLocation = iInitialHashLocation;
        // Keep track of our number of attempts:
        int iAttempts = 1;
        // Keep track of whether or not we've found it:
        bool bRemoved = false;

        // Keep looping until either we find the tiem or we hit a null:
        while (!bRemoved && oDataArray[iCurrentLocation] != null)
        {
            if (oDataArray[iCurrentLocation].GetType() == typeof(KeyValue<K, V>))
            {
                KeyValue<K, V> kv = (KeyValue<K, V>)oDataArray[iCurrentLocation];
                if (kv.Key.Equals(key))
                {
                    // If the keys match we've found the item:
                    vReturn = kv.Value;
                    bRemoved = true;
                    // Remove it by replacing it with a tombstone:
                    oDataArray[iCurrentLocation] = new Tombstone();
                    iCount--;
                    iTombstone++;
                }
            }
            // Advance to the next position:
            iCurrentLocation = iInitialHashLocation + GetIncrement(iAttempts++, key);
            // Mod by the table size so we loop around the table:
            iCurrentLocation %= HTSize;
        }

        // If we don't find the key, throw an ApplicationException:
        if (!bRemoved)
        {
            throw new ApplicationException("Key " + key + " not found in table.");
        }


        return vReturn;
    }

    // The easiest class you will ever write:
    internal class Tombstone
    {

    }

    private class OpenAddressEnumerator : IEnumerator<V>
    {
        private A_OpenAddressing<K, V> table;

        // Keep a reference to the Node we've most recently visited:
        private int index;


        public OpenAddressEnumerator(A_OpenAddressing<K, V> table)
        {
            this.table = table;
            Reset();
        }

        public V Current => ((KeyValue<K, V>)table.oDataArray[index]).Value;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            table = null;
            index = -1;

        }

        public bool MoveNext()
        {
            index++;
            //we must move until we hit a KV or exceed the length of the Array
            while (index < table.oDataArray.Length && !(table.oDataArray[index] is KeyValue<K, V>))
            {
                index++;
            }
            // if index is greater than length we have exceeded the size of the array and cannot move anymore
            return index < table.oDataArray.Length;
        }

        public void Reset()
        {
            // Like in the array_list enum, set the enum to be looking before the start of the collection
            index = -1;

        }
    }
}
