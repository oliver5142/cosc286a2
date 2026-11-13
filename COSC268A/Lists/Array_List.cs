using System;
using System.Collections;
using System.Collections.Generic;

namespace Lists;

/// <summary>
/// An implementation of a list using arrays
/// </summary>
/// <typeparam name="T">The type the list stores</typeparam>
public class Array_List<T>: A_List<T> where T: IComparable<T>
{
    // private attribute to actually hold our values, initialized to empty:
    private T[] values = new T[0];

    public override int Count
    {
        get 
        {
            return values.Length;
        }
    }

    public override void Add(T data)
    {
        // To add, we'll need to create a new array one bigger than the old one:
        T[] newValues = new T[values.Length+1];
        // Loop through the old array and copy the values over
        for (int i = 0; i < values.Length; i++)
        {
            newValues[i] = values[i];
        }
        // The last slot of the new array, which doesn't have anything in it yet,
        // is where we'll add the new value:
        newValues[newValues.Length - 1] = data;
        // don't forget to set our internal values to the new array:
        values = newValues;
    }

    public override void Clear()
    {
        values = new T[0];
    }

    public override IEnumerator<T> GetEnumerator()
    {
        return new Enumerator(this);
    }

    public override int IndexOf(T data)
    {
        // For indexof we'll just iterate through the array until we find the item
        // we're looking for
        for (int i = 0;i < values.Length;i++)
        {
            if (data.Equals(values[i]))
            {
                return i;
            }
        }
        throw new ApplicationException("Could not find item " + data + " in list.");
    }

    public override void Insert(int index, T data)
    {
        if (index < 0 || index >= values.Length)
        {
            throw new IndexOutOfRangeException("Index " + index + " out of range of list");
        }

        // Make a new bigger array
        T[] newValues = new T[values.Length + 1];
        // Loop through the old array, copying values over
        int j = 0; // The index for the new array
        // i is the index for the old array
        for (int i = 0; i < values.Length; i++)
        {
            //   When you get to the insertion index, add the new value
            //   (after this point, the indexes in the old array and new array will be out of sync)
            // If we're at the insertion point, we'll do something different:
            if (i == index)
            {
                // Insert the data at this position:
                newValues[j] = data;
                j++;
            }
            // Copy the old values over:
            newValues[j] = values[i];
            // Increment position in new array:
            j++;
        }

        // Finally set our internal values to point at the new array:
        values = newValues;
    }

    public override bool Remove(T data)
    {
        try
        {
            int indexOfElementToRemove = IndexOf(data);
            RemoveAt(indexOfElementToRemove);
            return true;
        }
        catch (ApplicationException e)
        {
            return false;
        }

    }

    public override T RemoveAt(int index)
    {
        // Keep track of the element removed (because we need to return it)
        T tReturn = default(T);

        // Create a new array one shorter than the last one
        T[] newValues = new T[values.Length - 1];

        // Loop through the old array copying every value except the one being replaced
        // Keep track of position in the new array as well!
        int j = 0;
        for (int i = 0; i < values.Length; i++)
        {
            if (i == index)
            {
                tReturn = values[i];
            }
            else
            {
                newValues[j++] = values[i];
            }
        }

        // Don't forget to set values to the new array
        values = newValues;
        // Return the item that was removed
        return tReturn;

    }

    public override T ReplaceAt(int index, T data)
    {
        T tReturn = values[index];
        values[index] = data;
        return tReturn;
    }

    private class Enumerator: IEnumerator<T>
    {
        // keep track of a reference to the outer class we're enumerating:
        private Array_List<T> list;
        // Keep track of where in the array we're looking at:
        private int index;

        // The constructor is just going to store that reference to the outer class:
        public Enumerator(Array_List<T> l)
        {
            this.list = l;
            Reset();
        }

        // Return the current value the enumerator is pointing at
        public T Current
        {
            get
            {
                return list.values[index];
            }
        }

        object IEnumerator.Current => Current;

        // Just free up any references so they can be GC'd
        public void Dispose()
        {
            list = null;
        }

        // Reset should set the enumerator to be looking at just before the start
        // of the collection
        public void Reset()
        {
            index = -1;
        }

        // MoveNext moves to the next item in the collection if it can
        // Returns true if it can, false if it can't
        public bool MoveNext()
        {
            if ((index + 1) < list.values.Length)
            {
                index++;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
