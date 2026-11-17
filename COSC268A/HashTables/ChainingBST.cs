using BinarySearchTrees;
using System;
using System.Collections;
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
        public int getBucketCount()
        {
            return iBucketCount;
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
                // Check if there's an BST in that slot:
                if (oOldDataArray[i] != null)
                {
                    // If there is an BST, I'll grab it, cast it, and go through it:
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

            // We'll grab the BST from that slot in the table
            BST<KeyValue<K, V>> bstCurrent = (BST<KeyValue<K, V>>)oDataArray[iHash];

            // It may or may not be in that BST, so we may want to keep track
            // of whether we find it in the BST
            KeyValue<K, V> bKeyFound = new KeyValue<K, V>(key, default(V));

            // If the BST isn't null (so that bucket in the hashtable wasn't empty)
            if (bstCurrent != null)
            {
                // Search through the BST for the KeyValue pair where
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
        public override string ToString()
        {
            int index = 0;
            BST<KeyValue<K, V>> bucket = null;
            string sReturn = "";


            // We will iterate through the whole table and if we hit a bucket we will call the
            // bst iterator with our AddToString function to process the data we find
            while (index < this.oDataArray.Length - 1)
            {
                index++;
                if (this.oDataArray[index] is not null)
                {
                    bucket = (BST<KeyValue<K, V>>)this.oDataArray[index];
                    sReturn += "[";
                    bucket.Iterate(AddToString, TRAVERSALORDER.IN_ORDER);
                    sReturn += "], ";
                }
                else
                {
                    sReturn += "[empty], ";
                }
            }
            return sReturn;
            // bucket.Iterate will give us KeyValue<K, V> data to process
            void AddToString(KeyValue<K, V> kv)
            {
                sReturn += "Key: " + kv.Key.ToString() + " Value: " + kv.Value.ToString();
            }

        }

        public override IEnumerator<V> GetEnumerator()
        {
            return new ChainingBSTEnumerator(this);
        }

        private class ChainingBSTEnumerator : IEnumerator<V>
        {
            private ChainingBST<K, V> table;
            // Every bucket in the table contains a BST so we must keep a variable for the current bucket
            private BST<KeyValue<K, V>> bucket;
            // We will be using the BST Enumerator to enumerate the buckets when we hit one
            private IEnumerator<KeyValue<K, V>> bucketEnum;

            // Keep a reference to the indexes we've most recently visited:
            private int index;


            public ChainingBSTEnumerator(ChainingBST<K, V> table)
            {
                this.table = table;
                Reset();
            }

            public V Current
            {
                get
                {
                    return bucketEnum.Current.Value;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                table = null;
                index = -1;
                bucketEnum = null;
            }

            public bool MoveNext()
            {
                // first check if we are currently inside a bucket
                if (bucket is not null)
                {
                    // if the bucket is able to move we will return true
                    if (bucketEnum.MoveNext())
                    {
                        return true;
                    }
                    // bucket has been fully traversed we must set it to null
                    else
                    {
                        bucket = null;
                    }
                }

                // then check if the table has been fully traversed. if it hasn't been then advance until we hit another bucket
                // or we go off the edge. 
                while (index < table.oDataArray.Length - 1 && bucket is null)
                {
                    index++;
                    // check if there is a bucket
                    // if there is then initialize the enum and return true
                    if (table.oDataArray[index] is not null)
                    {
                        bucket = (BST<KeyValue<K, V>>)table.oDataArray[index];
                        bucketEnum = bucket.GetEnumerator();
                        bucketEnum.Reset();
                        bucketEnum.MoveNext();
                        return true;
                    }
                }

                // We made it to the end of the table
                return false;
            }

            // reset variables so we can enumerate the table again
            public void Reset()
            {
                index = -1;
                bucketEnum = null;
                bucket = null;
            }
        }

        public override V Remove(K key)
        {
            // We'll grab the hash location where in the table we should look
            int iHash = HashFunction(key);

            // We'll grab the BST from that slot in the table
            BST<KeyValue<K, V>> bstCurrent = (BST<KeyValue<K, V>>)oDataArray[iHash];

            // It may or may not be in that BST, so we may want to keep track
            // of whether we find it in the BST
            KeyValue<K, V> bKeyFound = new KeyValue<K, V>(key, default(V));

            // If the BST isn't null (so that bucket in the hashtable wasn't empty)
            if (bstCurrent != null)
            {
                // Search through the BST for the KeyValue pair where
                // the key matches the key we are looking for
                bKeyFound = bstCurrent.Find(new KeyValue<K, V>(key, default(V)));
            }
            // If we haven't found the KeyValue pair we're looking for, throw
            // an ApplicationException to say not found
            if (bKeyFound.Value is null)
            {
                throw new ApplicationException("Could not find key " + key + " in hashtable.");
            }

            // Remove the kv from the BST then check if the BST has any more elements
            // if it doesn't set the location to null and decrease bucketCount
            bstCurrent.Remove(bKeyFound);
            iCount--;
            if (bstCurrent.Count == 0)
            {
                oDataArray[iHash] = null;
                iBucketCount--;
            }

            // Otherwise, return the Value corresponding to the key
            return bKeyFound.Value;
        }
    }
}
