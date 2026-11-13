using System;
using System.Collections.Generic;

namespace HashTables;

// K ->> Generic type representing the data type of the key
// V ->> Generic type representing the data type of the value
public interface I_HashTable<K, V>: IEnumerable<V> where K: IComparable<K>
{

    /// <summary>
    /// A sorted array of all the keys in the hashtable.
    /// </summary>
    public K[] Keys { get; }

    /// <summary>
    /// Return the value mapped to a given key
    /// </summary>
    /// <param name="key">The key to look up</param>
    /// <returns>The value mapped to that key</returns>
    V Get(K key);

    /// <summary>
    /// Map the value to the key in the hashtable. Adds the keyvalue pair.
    /// </summary>
    /// <param name="key">The key is used to retrieve the pair and determine its location</param>
    /// <param name="value">The value to stort at this location.</param>
    // If we weren't writing this generic, and you know in advance what type of object
    // you're storing in the hashtable, and you know the key is going to be one property
    // of the object, you could just pass in the object.
    // For instance, if I was storing objects of type Student, and using the Student's
    // StudentNumber property as the key, I could just write a Add(Student s) method instead.
    void Add(K key, V value);

    /// <summary>
    /// Remove the value associated with the key - unmap them.
    /// </summary>
    /// <param name="key">The key to be unmapped</param>
    /// <returns>The value removed</returns>
    V Remove(K key);

    /// <summary>
    /// Removes all key-value pairs from the hashtable and initializes to the default
    /// array size.
    /// </summary>
    void Clear();
}
