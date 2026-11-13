using System;
namespace HashTables;

/// <summary>
/// A linear hashtable is a specific type of Open Addressing table
/// that uses a linear probe sequence - we just add a set number to the index
/// on every attempt to put the item in the table
/// </summary>
public class LinearHT<K,V>: A_OpenAddressing<K,V> where K: IComparable<K>
{
    // We'll store the increment amount in a constant - this is how
    // many steps to look ahead:
    private int iIncrement = 1;

    protected override int GetIncrement(int iAttempt, K key)
    {
        // For linear, the key doesn't matter
        // For linear the amount to add is just i (attempts) * c (our increment):
        return iIncrement * iAttempt;
    }
}
