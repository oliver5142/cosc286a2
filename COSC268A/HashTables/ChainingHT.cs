using Lists;
using System;
using System.Collections;
using System.Collections.Generic;

namespace HashTables;

public class ChainingHT<K, V> : A_HashTable<K, V> where K : IComparable<K>
{
    public override K[] Keys => throw new NotImplementedException();

    // Let's set an initial table size:
    private const int iInitialSize = 4;

    // Keep track of how many buckets have something in it
    int iBucketCount = 0;

    // We need a constructor because we need to initialize the object array!
    public ChainingHT()
    {
        oDataArray = new object[iInitialSize];
    }

    public override void Add(K key, V value)
    {
        // First we'll have to find what bucket/table location in the table
        // we're adding to - thankfully we already wrote a HashFunction for that!
        int iHash = HashFunction(key);

        // Bundle the key and value into a KeyValue object:
        KeyValue<K, V> keyValue = new KeyValue<K, V>(key, value);

        // We'll store these in ArrayLists as the secondary structure inside each bucket
        // We'll declare (but not initialize) the ArrayList we're going to put things in:
        Array_List<KeyValue<K, V>> alCurrent;

        // Do I need to create that list, or does it already exist?
        // If there's something in that slot in the hashtable, we know the list exists
        // If that bucket is null, we know we need to create it
        if (oDataArray[iHash] == null)
        {
            // The bucket is empty, we need to create the list:
            alCurrent = new Array_List<KeyValue<K, V>>();
            // Put it in the right bucket:
            oDataArray[iHash] = alCurrent;
            // Keep track of how many buckets have something:
            iBucketCount++;
        }
        else
        {
            // If there is an array list, grab it and cast it:
            alCurrent = (Array_List<KeyValue<K, V>>)oDataArray[iHash];
            // We should double check our key isn't already in the list
            // because we should not allow duplicates in a hashtable:
            if (alCurrent.Contains(keyValue))
            {
                throw new ApplicationException("Key already exists in the hashtable");
            }
            // Since we're putting something in an existing bucket, this is a collision:
            iNumCollisions++;
        }

        // Either way, we're going to end up with alCurrent being an arraylist
        // in the hashtable and we can add our KeyValue pair to it:
        alCurrent.Add(keyValue);
        // Don't forget to incrmeent your count:
        iCount++;

        // Check if the hashtable is overloaded, and expand if we are:
        if (isOverloaded())
        {
            expandHashTable();
        }
    }

    /// <summary>
    /// Check if we're overloaded. We're overloaded if our count divided by
    /// the table size is greater than the load factor limit
    /// </summary>
    /// <returns>True if overloaded, false otherwise</returns>
    private bool isOverloaded()
    {
        double currentLoadFactor = (double)(iCount / HTSize);
        bool isOverLoaded = currentLoadFactor > dLoadFactorLimit;
        return isOverLoaded;
    }

    /// <summary>
    /// Expands the hashtable by creating a new, bigger hashtable
    /// and adding everything in the old table to it
    /// </summary>
    private void expandHashTable()
    {
        // Get a reference to the old table array:
        object[] oOldDataArray = oDataArray;
        // Replace the oDataArray with a new array which is TWICE the size of the old
        oDataArray = new object[HTSize * 2];

        // Don't forget to reset all our counters - iCount, iBucketCount, iNumCollisions:
        iCount = 0;
        iBucketCount = 0;
        iNumCollisions = 0;

        // Go through everything in the old table array and add it to the hashtable again:
        for (int i = 0; i < oOldDataArray.Length; i++)
        {
            // Check if there's an arraylist in that slot:
            if (oOldDataArray[i] != null)
            {
                // If there is an Array_list, I'll grab it, cast it, and go through it:
                Array_List<KeyValue<K, V>> alCurrent = (Array_List<KeyValue<K, V>>)oOldDataArray[i];
                foreach (KeyValue<K, V> kv in alCurrent)
                {
                    // Just use the Add method to add it to our table:
                    Add(kv.Key, kv.Value);
                }
            }
        }

    }

    public override void Clear()
    {
        iBucketCount = 0;
        iCount = 0;
        iNumCollisions = 0;
        oDataArray = new object[iInitialSize];
    }

    public override IEnumerator<V> GetEnumerator()
    {
        return new ChainingHTEnumerator(this);
    }

    public override V Get(K key)
    {
        V vReturn = default(V);
        // We'll grab the hash location where in the table we should look
        int iHash = HashFunction(key);

        // We'll grab the ArrayList from that slot in the table
        Array_List<KeyValue<K, V>> alCurrent = (Array_List<KeyValue<K, V>>)oDataArray[iHash];

        // It may or may not be in that array list, so we may want to keep track
        // of whether we find it in the arraylist
        bool bKeyFound = false;

        // If the arraylist isn't null (so that bucket in the hashtable wasn't empty)
        if (alCurrent != null)
        {
            // Search through the arraylist for the KeyValue pair where
            // the key matches the key we are looking for
            foreach (KeyValue<K, V> kv in alCurrent)
            {
                // If we find the key value pair we are looking for,
                if (kv.Key.Equals(key))
                {
                    // make note of the fact we've found the item so we can stop our search
                    // and grab the value to return
                    bKeyFound = true;
                    vReturn = kv.Value;
                    break;
                }
            }
        }

        // If we haven't found the KeyValue pair we're looking for, throw
        // an ApplicationException to say not found
        if (bKeyFound == false)
        {
            throw new ApplicationException("Could not find key " + key + " in hashtable.");
        }

        // Otherwise, return the Value corresponding to the key
        return vReturn;
    }

    public override V Remove(K key)
    {
        // Keep track of the item removed and if we were able to remove it or not
        KeyValue<K, V> kvRemoved = null;

        // Use the hashfunction to get the location of the item, if it exists
        int iHashLocation = HashFunction(key);
        // Grab the arraylist in that location
        Array_List<KeyValue<K, V>> alCurrent = (Array_List<KeyValue<K, V>>)oDataArray[iHashLocation];

        // If the arraylist exists:
        if (alCurrent != null)
        {
            // Go through the arraylist element by element looking for the item:
            for (int i = 0; i < alCurrent.Count; i++)
            {
                // If I find the KeyValue pair whose key matches the key to remove
                if (alCurrent.ElementAt(i).Key.Equals(key))
                {
                    //  remove if from the arraylist
                    kvRemoved = alCurrent.RemoveAt(i);
                    // Don't forget to decrement the count
                    iCount--;
                    // If the arraylist is now empty, remove it from the table
                    //    and decrment the bucket count
                    if (alCurrent.Count == 0)
                    {
                        oDataArray[iHashLocation] = null;
                        iBucketCount--;
                    }
                }

            }
        }

        // If we didn't find the item, throw an ApplicationException
        if (kvRemoved == null)
        {
            throw new ApplicationException("Key " + key + " doesn't exist in table to remove.");
        }

        // If we did, return the value of the keyvalue pair we found
        return kvRemoved.Value;
    }

    private class ChainingHTEnumerator : IEnumerator<V>
    {
        private ChainingHT<K, V> table;
        private Array_List<KeyValue<K, V>> bucket;

        // Keep a reference to the indexes we've most recently visited:
        private int index, bucketIndex;


        public ChainingHTEnumerator(ChainingHT<K, V> table)
        {
            this.table = table;
            Reset();
        }

        public V Current
        {
            get
            {
                return bucket.ElementAt(bucketIndex).Value;
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            table = null;
            index = -1;
            bucketIndex = -1;


        }

        public bool MoveNext()
        {
            // first check if we are currently inside a bucket
            if (bucket is not null)
            {
                if (bucketIndex + 1 < bucket.Count)
                {
                    bucketIndex++;
                    return true;
                }
                else
                {
                    bucket = null;
                    bucketIndex = -1;
                }
            }

            // then check if the table has been fully traversed. if it hasn't been then advance until we hit another bucket
            // or we go off the edge. 
            while (index < table.oDataArray.Length - 1 && bucket is null)
            {
                index++;
                //check if there is a bucket
                if (table.oDataArray[index] is not null)
                {
                    bucket = (Array_List<KeyValue<K, V>>)table.oDataArray[index];
                    bucketIndex++;
                    return true;
                }
            }
            //then if we pass both of those checks we return false

            return false;
        }

        // reset variables so we can enumerate the table again
        public void Reset()
        {
            index = -1;
            bucketIndex = -1;
            bucket = null;
        }
    }
}
