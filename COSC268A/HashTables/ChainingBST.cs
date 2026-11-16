using BinarySearchTrees;
using System;
using System.Collections.Generic;

namespace HashTables
{
    public class ChainingBST<K, V> : A_HashTable<K, V> where K : IComparable<K>
    {

        // Let's set an initial table size (5) and the factor by which it will expand (2):
        private const int iExpansionFactor = 2;
        private const int iInitialSize = 5;
        // Keep track of how many buckets have something in it
        int iBucketCount = 0;

        // Constructor to initialize object array
        public ChainingBST()
        {
            oDataArray = new object[iInitialSize];
        }


        public override K[] Keys => throw new NotImplementedException();

        public override void Add(K key, V value)
        {

            // First we'll have to find what bucket/table location in the table
            // we're adding to - thankfully we already wrote a HashFunction for that!
            int iHash = HashFunction(key);

            // Bundle the key and value into a KeyValue object:
            KeyValue<K, V> keyValue = new KeyValue<K, V>(key, value);

            // We'll store these in BST as the secondary structure inside each bucket
            // We'll declare (but not initialize) the BST we're going to put things in:
            BST<KeyValue<K, V>> bstCurrent;

            if (oDataArray[iHash] == null)
            {
                // The bucket is empty, we need to create the list:
                bstCurrent = new BST<KeyValue<K, V>>();
                // Put it in the right bucket:
                oDataArray[iHash] = bstCurrent;
                // Keep track of how many buckets have something:
                iBucketCount++;
            }
            else
            {
                // If there is an array list, grab it and cast it:
                bstCurrent = (BST<KeyValue<K, V>>)oDataArray[iHash];
                // We should double check our key isn't already in the list
                // because we should not allow duplicates in a hashtable:
                if (bstCurrent.Contains(keyValue))
                {
                    throw new ApplicationException("Key already exists in the hashtable");
                }
                // Since we're putting something in an existing bucket, this is a collision:
                iNumCollisions++;
            }

            // Either way, we're going to end up with alCurrent being an arraylist
            // in the hashtable and we can add our KeyValue pair to it:
            bstCurrent.Add(keyValue);
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
            oDataArray = new object[HTSize * iExpansionFactor];

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
                    BST<KeyValue<K, V>> alCurrent = (BST<KeyValue<K, V>>)oOldDataArray[i];
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

        public override V Get(K key)
        {
            // We'll grab the hash location where in the table we should look
            int iHash = HashFunction(key);

            // We'll grab the ArrayList from that slot in the table
            BST<KeyValue<K, V>> bstCurrent = (BST<KeyValue<K, V>>)oDataArray[iHash];

            // It may or may not be in that array list, so we may want to keep track
            // of whether we find it in the arraylist
            KeyValue<K, V> bKeyFound = new KeyValue<K, V>(key, default(V));

            // If the arraylist isn't null (so that bucket in the hashtable wasn't empty)
            if (bstCurrent != null)
            {
                // Search through the arraylist for the KeyValue pair where
                // the key matches the key we are looking for
                bKeyFound = bstCurrent.Find(new KeyValue<K, V>(key, default(V)));
            }


            // If we haven't found the KeyValue pair we're looking for, throw
            // an ApplicationException to say not found
            if (bKeyFound.Value is null)
            {
                throw new ApplicationException("Could not find key " + key + " in hashtable.");
            }

            // Otherwise, return the Value corresponding to the key
            return bKeyFound.Value;
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
