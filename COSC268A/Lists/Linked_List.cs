using System;
using System.Collections;
using System.Collections.Generic;

namespace Lists;

public class Linked_List<T> : A_List<T> where T : IComparable<T>
{
    // A reference to the head of the Linked List:
    private Node head;

    public override void Add(T data)
    {
        // Just call the recursive add method:
        //head = recAdd(head, data);

        // Let us try writing add iteratively instead!
        // If the list is empty, that's easy - just create a new node and point head at it
        if (head == null)
        {
            head = new Node(data);
        }
        else
        {
            // Otherwise:
            //   Progress through the list going from next to next until you find the last node
            Node current = head;
            //   The last node's next value should be null
            while (current.next != null)
            {
                current = current.next;
            }
            //   Once you reach a node whose next is null, create a new node, and set the old
            //     last node's next value to point at the new node
            current.next = new Node(data);
        }

    }

    /// <summary>
    /// Recursive helper method for add.
    /// Takes in a node and a data item to add to the end of the chain
    /// </summary>
    /// <param name="current">The current node we're looking at</param>
    /// <param name="data">The data to add to the end of the list</param>
    /// <returns>The current node</returns>
    private Node recAdd(Node current, T data)
    {
        // Base case is that we're at the end of the chain:
        if (current == null)
        {
            // Add in the new data
            current = new Node(data);
        }
        else
        {
            // The recursive case is that we're not at the end of the list:
            current.next = recAdd(current.next, data);
        }

        return current;
    }

    public override void Clear()
    {
        head = null;
    }

    public override IEnumerator<T> GetEnumerator()
    {
        return new LinkedListEnumerator(this);
    }

    public override int IndexOf(T data)
    {
        Node current = head;
        int index = 0;
        while (current != null)
        {
            // If we've found a match:
            if (current.data.CompareTo(data) == 0)
            {
                // return the index:
                return index;
            }
            // Otherwise, just go ahead to the next node:
            index++;
            current = current.next;
        }

        // If current ever equals null, it means we could not find the item:
        throw new ApplicationException("Could not find item " + data + "in list.");
    }

    public override void Insert(int index, T data)
    {
        // Whether we're doing it recursively or iteratively, we need to:
        // First it is easiest to do a bounds check
        if (index > this.Count || index < 0)
        {
            throw new IndexOutOfRangeException("Index " + index + " is out of bounds.");
        }
        // Progress forward through the list until we find
        // where to insert the new item
        // Then we need to make the parent of the node at the position
        // we're inserting in have the NEW NODE be its next node,
        // and make our new node's next be the old next of the parent

        // First I'll do a look ahead recursive solution
        // We're going to treat current node as the parent, and we're
        // looking ahead to see if we need to insert into the next position
        // If we're inserting at 0, I have to treat head as a special case:
        /*if (index == 0)
        {
            head = new Node(data, head);
        }
        else
        {
            // We'll recurse until we find the parent of the position to
            // insert at:
            recInsert(index, head, data);
        }*/

        // If we were to do this not looking ahead, we'd
        // have to either pass using ref or rebuild the list so we can keep track
        // of changed references:
        head = recInsert(index, head, data);
    }


    private Node recInsert(int index, Node current, T data)
    {
        // Base case is if we're inserting in the current position:
        if (index == 0)
        {
            // Create the new node, make the current node its next
            current = new Node(data, current);
        }
        else
        {
            // Recursive case, we're not inserting yet:
            current.next = recInsert(--index, current.next, data);
        }

        return current;
    }

    /*private void recInsert(int index, Node current, T data)
    {
        // We're looking ahead,
        // So my base case is if the insert position is the *next* position
        // If it is, create a new node whose next is the current node's next
        //    and make the current node's next the new node:
        if (index == 1)
        {
            current.next = new Node(data, current.next);
        }
        // Otherwise I'll decrement the index and recurse on the next node:
        else
        {
            recInsert(--index, current.next, data);
        }
    }*/

    /*public override bool Remove(T data)
    {
        // Let us try Remove with an iterative version:
        // Keep track of whether or not we were able to find the item to remove:
        bool bRemoved = false;

        // If the list is empty, there's nothing to remove:
        if (head == null)
        {
            return bRemoved;
        }

        // Next case we need to consider - if the data we're removing is at
        // the head of the list:
        if (head.data.CompareTo(data) == 0)
        {
            // Make head's next the new head and note we were able to remove head
            head = head.next;
            bRemoved = true;
        }
        else
        {
            // If it is not at the head of the list, we need to use a loop
            // to move through the list. Since we're singly linked, we need to keep
            // track of not only node we want to remove, but also its previous neighbour
            Node currentNode = head;
            Node scoutNode;
            // We'll keep looping while two things are true:
            // * we haven't removed the item yet
            // * there are still items in the list we haven't looked at
            while (!bRemoved && currentNode != null)
            {
                // Keep track of the next node in the sequence:
                scoutNode = currentNode.next;
                // Is the next node in the sequence the one I need to remove?
                if (scoutNode != null && scoutNode.data.CompareTo(data) == 0)
                {
                    // We'll set our current node's next to be the target node's next
                    bRemoved = true;
                    currentNode.next = scoutNode.next;
                }
                else
                {
                    // If we didn't find the item to remove, keep going down the chain:
                    currentNode = scoutNode;
                }
            }
        }

        // Return if we were able to successfully remove or not:
        return bRemoved;
    }*/

    // Let us try a recursive version of Remove:
    public override bool Remove(T data)
    {
        return recRemove(ref head, data);
    }

    private bool recRemove(ref Node current, T data)
    {
        // We'll need to keep track of whether or not the node was found and removed
        bool bRemoved = false;

        // We have two base cases to consider:
        // One base case is current is null, which means the item isn't present and can't be removed
        if (current == null)
        {
            return bRemoved;
        }

        // The other base case is we've FOUND the item to remove
        if (current.data.CompareTo(data) == 0)
        {
            //    If we've found it, mark that we've found it
            bRemoved = true;
            //    Cut out the item to remove
            current = current.next;
        }
        else
        {
            // If neither of those base cases were met, recurse on the next node
            bRemoved = recRemove(ref current.next, data);
        }

        // We always return whether or not we were able to remove it eventually
        return bRemoved;
    }

    public override T RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    public override T ReplaceAt(int index, T data)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// A single node in a linked list which stores data of type T
    /// </summary>
    private class Node
    {
        public T data;
        public Node next;

        public Node(T data, Node next)
        {
            this.data = data;
            this.next = next;
        }

        public Node(T data): this(data, null) {  }

    }

    private class LinkedListEnumerator : IEnumerator<T>
    {
        // We'll keep a reference to the linked list we're enumerating:
        private Linked_List<T> list;

        // Keep a reference to the Node we've most recently visited:
        private Node lastVisited;
        // Keep a reference to the next node we're going to visit
        private Node scout;

        public T Current
        {
            get
            {
                // Return the value of the node we're currently looking at:
                return lastVisited.data;
            }
        }

        object IEnumerator.Current => Current;

        public LinkedListEnumerator(Linked_List<T> list)
        {
            this.list = list;
            Reset();
        }

        public void Dispose()
        {
            list = null;
            scout = null;
            lastVisited = null;
        }

        public bool MoveNext()
        {
            bool bWasAbleToMove = false;

            if (scout != null)
            {
                bWasAbleToMove = true;
                // Since there's a next node, advance our lastvisited to point at
                // next, and advance scout to point at the one after:
                lastVisited = scout;
                scout = lastVisited.next;
            }

            // We'll return true if we were able to move, false otherwise:
            return bWasAbleToMove;
        }

        public void Reset()
        {
            // Remember a Reset call should point us at before the start of the collection
            // Which means we aren't currently visiting any nodes:
            lastVisited = null;
            // The next node we're going to visit - the scout - is going to be the head of
            // the list:
            scout = list.head;
        }
    }
}
