using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BinarySearchTrees;

public class BST<T> : A_BST<T>, ICloneable where T : IComparable<T>
{
    public BST()
    {
        // Initialize the root:
        nRoot = null;
        // Set the count to 0:
        iCount = 0;
    }

    public override void Add(T data)
    {
        // Where do we add a new node? Where it belongs!
        // The really easy case is when the tree is empty:
        if (nRoot == null)
        {
            nRoot = new Node<T>(data);
        }
        else
        {
            // Otherwise we'll use recursion to solve this:
            recAdd(data, nRoot);
            nRoot = Balance(nRoot);
        }
        iCount++;
    }

    // Helper method to recursively add a Node to a tree:
    private void recAdd(T data, Node<T> nCurrent)
    {
        // Compare the data we're adding to the current node and see if the new
        // data belongs to the left or right:
        int iCompare = data.CompareTo(nCurrent.Data);

        // If the data we're adding belongs on the left (iCompare < 0):
        if (iCompare < 0)
        {
            // Base case: it belongs on the left, and nCurrent doesn't have a left child
            if (nCurrent.Left == null)
            {
                // Create the new node and add it as the current Node's left child
                nCurrent.Left = new Node<T>(data);
            }
            else
            {
                // Recursive case: It belongs on the left, but nCurrent already has a left child:
                // Recursively call on our left child
                recAdd(data, nCurrent.Left);
                nCurrent.Left = Balance(nCurrent.Left);
            }

        }
        else
        {
            // Otherwise it belongs on the right 
            // Base case: it belongs on the right, and nCurrent doesn't have a right child
            if (nCurrent.Right == null)
            {
                // Create the new node and add it as the current Node's right child
                nCurrent.Right = new Node<T>(data);
            }
            else
            {
                // Recursive case: It belongs on the right, but nCurrent already has a right child:
                // Recursively call on our right child
                recAdd(data, nCurrent.Right);
                nCurrent.Right = Balance(nCurrent.Right);
            }
        }
    }

    // A virtual Balance method that our children can ovverride if they want
    // a hook into balancing on add
    internal virtual Node<T> Balance(Node<T> nCurrent)
    {
        return nCurrent;
    }

    public override void Clear()
    {
        nRoot = null;
        iCount = 0;
    }

    public object Clone()
    {
        throw new NotImplementedException();
    }

    public override T Find(T data)
    {
        return recFind(data, nRoot);
    }

    protected T recFind(T data, Node<T> nCurrent)
    {
        T tReturn = default(T);
        // First base case - if nCurrent is null, the item isn't in the tree
        if (nCurrent == null)
        {
            //     So throw a new ApplicationException saying we haven't found it
            throw new ApplicationException("Item " + data + " not in tree.");
        }
        else
        {
            // Otherwise, if nCurrent isn't null...
            //     Use CompareTo to check where the data item we're looking for will be
            int iResult = data.CompareTo(nCurrent.Data);
            //     Second Base Case - if the compareto is 0, we've found our item! So return the current item.
            if (iResult == 0)
            {
                tReturn = nCurrent.Data;
            }
            else if (iResult < 0)
            {
                //     Otherwise if it is less than, recurse on the lefthand of the tree
                tReturn = recFind(data, nCurrent.Left);
            } else
            {
                //     Otherwise if it is greater than, recurse on the righthand of the tree
                tReturn = recFind(data, nCurrent.Right);
            }
        }
        // Return the found object:
        return tReturn;
    }

    /// <summary>
    /// Returns the data of the node furthest right in the tree
    /// </summary>
    /// <returns>The largest node in the tree</returns>
    public T FindLargest()
    {
        // If the root is null the tree is empty and there is no largest, so exception:
        if (nRoot == null)
        {
            throw new ApplicationException("Root is null, can't find largest in empty tree");
        }
        else
        {
            return recFindLargest(nRoot);
        }
    }
    
    protected T recFindLargest(Node<T> nCurrent)
    {
        // Keep going right until you can't go right anymore
        T tReturn;
        if (nCurrent.Right == null)
        {
            // If we have no right child, we can't go further right, so we're the biggest
            tReturn = nCurrent.Data;
        }
        else
        {
            // If we do have a right child, we'll just return our largest on the right
            tReturn = recFindLargest(nCurrent.Right);
        }
        return tReturn;
    }

    /// <summary>
    /// Returns the data of the node furthest left in the tree
    /// </summary>
    /// <returns>The smallest node in the tree</returns>
    public T FindSmallest()
    {
        // If the root is null the tree is empty and there is no smallest, so exception:
        if (nRoot == null)
        {
            throw new ApplicationException("Root is null, can't find smallest in empty tree");
        }
        else
        {
            return recFindSmallest(nRoot);
        }
    }

    protected T recFindSmallest(Node<T> nCurrent)
    {
        // Keep going left until you can't go left anymore
        if (nCurrent.Left == null)
        {
            // If we have no left child, we can't go further left so we're the smallest:
            return nCurrent.Data;
        }
        else
        {
            // If we do have a lefthand child, we'll just return the smallest on the left:
            return recFindSmallest(nCurrent.Left);
        }
    }

    public override IEnumerator<T> GetEnumerator()
    {
        return new DepthFirstEnumerator(this);
    }

    public IEnumerator<T> GetBreadthFirstEnumerator()
    {
        return new BreadthFirstEnumerator(this);
    }

    // The height of the tree is the number of edges from the deepest leaf node
    // to root node
    // If there is only a root node and no children, the height will be 0
    // If there is no root node - a completely empty tree - we'll say height is -1
    public override int Height()
    {
        // Easy case: if the tree is empty, height is -1
        int iHeight = -1;
        // Almost as easy, if the root node has not children, the height is 0
        // If the root node HAS children, the height of the tree will be equal to
        // the greater of the height on the left and the height on the right PLUS 1
        // How do we find out the height of the left child?
        //   If the left child has no children, its height is 0!
        //   If the left child has at least one child, its height will be equal to
        //     the height of the tallest child plus one
        // And so on
        if (nRoot != null)
        {
            iHeight = recHeight(nRoot);
        }

        return iHeight;
    }

    protected static int recHeight(Node<T> nCurrent)
    {
        int iHeightLeft = 0;
        int iHeightRight = 0;

        if (nCurrent.Left != null)
        {
            iHeightLeft = recHeight(nCurrent.Left) + 1;
        }
        if (nCurrent.Right != null)
        {
            iHeightRight = recHeight(nCurrent.Right) + 1;
        }
        int iGreaterHeight = iHeightLeft > iHeightRight ? iHeightLeft : iHeightRight;
        return iGreaterHeight;
    }

    public override void Iterate(ProcessData<T> pd, TRAVERSALORDER order)
    {
        if (nRoot != null)
        {
            recIterate(nRoot, pd, order);
        }
    }

    private void recIterate(Node<T> nCurrent, ProcessData<T> pd, TRAVERSALORDER to)
    {
        // PRE-ORDER:
        // First run pd with the root node
        if (to == TRAVERSALORDER.PRE_ORDER)
        {
            pd(nCurrent.Data);
        }        

        // Then iterate on the left subtree
        if (nCurrent.Left != null)
        {
            recIterate(nCurrent.Left, pd, to);
        }

        // IN-ORDER:
        // Run pd on the root between the left and the right:
        if (to == TRAVERSALORDER.IN_ORDER)
        {
            pd(nCurrent.Data);
        }

        // Then iterate on the right subtree
        if (nCurrent.Right != null)
        {
            recIterate(nCurrent.Right, pd, to);
        }

        // POST-ORDER:
        // Run pd on the root after running on the left and the right:
        if (to == TRAVERSALORDER.POST_ORDER)
        {
            pd(nCurrent.Data);
        }
    }

    public override bool Remove(T data)
    {
        // We do need to keep track of whether or not we removed the node:
        bool bRemoved = false;
        // We need to alter the tree to remove the node:
        nRoot = recRemove(nRoot, data, ref bRemoved);
        // I passed bRemoved in as a ref so the method can change its value

        return bRemoved;
    }

    private Node<T> recRemove(Node<T> nCurrent, T data, ref bool bRemoved)
    {
        // We only need to do something if the current node isn't null. If it is null,
        // we know we couldn't find the node to remove, so bRemoved can stay false
        if (nCurrent != null)
        {
            // If the current node is NOT null, check if it is the node we want to remove:
            int iCompare = data.CompareTo(nCurrent.Data);
            if (iCompare < 0)
            {
                // If it isn't the right node, and the node we want to remove is on the left,
                // recurse on the left:
                nCurrent.Left = recRemove(nCurrent.Left, data, ref bRemoved);
            }
            else if (iCompare > 0) 
            {
                // If the node we want to remove is on the right, recurse on the right:
                nCurrent.Right = recRemove(nCurrent.Right, data, ref bRemoved);
            }
            else
            {
                // Base case: We've found the node to remove!
                bRemoved = true;
                // If it is a leaf node, it is easy to remove:
                if (nCurrent.IsLeaf())
                {
                    nCurrent = null;
                    iCount--;
                }
                else if (nCurrent.Left != null && nCurrent.Right != null)
                {
                    // If it has TWO children:
                    // We will need to pull up a node from beneath us to take our place.
                    // What nodes are viable candidates? Either the biggest node on the left,
                    // Or the smallest node on the right
                    // We'll use the largest on the left - so we need to grab the data value of it
                    T tReplacement = recFindLargest(nCurrent.Left);
                    // Replace the current node's data value with our replacement:
                    nCurrent.Data = tReplacement;
                    // Now of course that replacement node exists in the tree twice...
                    // So go ahead and remove the original replacement
                    nCurrent.Left = recRemove(nCurrent.Left, tReplacement, ref bRemoved);
                }
                else if (nCurrent.Left != null)
                {
                    nCurrent = nCurrent.Left;
                    iCount--;
                }
                else
                {
                    nCurrent = nCurrent.Right;
                    iCount--;
                }
            }
        }

        // return a reference to the current node, which may have changed:
        return nCurrent;
    }

    public override string ToString()
    {
        return "Tree: " + nRoot;
    }

    class DepthFirstEnumerator : IEnumerator<T>
    {
        // Store a reference to the tree we're enumerating:
        protected BST<T> tree;
        // Keep track of the current node inside the tree we're looking at:
        protected Node<T> nCurrent = null;
        // Keep track of future nodes to visit. Because I want to do this in a 
        // First In Last Out manner, that means we want a stack:
        protected Stack<Node<T>> sNodes;

        public T Current => nCurrent.Data;

        object IEnumerator.Current => Current;

        public DepthFirstEnumerator(BST<T> tree)
        {
            this.tree = tree;
            Reset();
        }

        public void Dispose()
        {
            tree = null;
            nCurrent = null;
            sNodes = null;
        }

        public bool MoveNext()
        {
            // Keep track of if we were able to move:
            bool bMoved = false;
            // If there's anything left in the stack (stack has a Count property we can check)
            if (sNodes.Count > 0)
            {
                // That means we *can* move to the next item
                bMoved = true;
                //     We'll grab the next node from the stack as our new current
                nCurrent = sNodes.Pop();
                //     If that node has any children, we'll add them to the stack
                //       First Right, then left
                if (nCurrent.Right != null)
                {
                    sNodes.Push(nCurrent.Right);
                }
                if (nCurrent.Left != null)
                {
                    sNodes.Push(nCurrent.Left);
                }
            }

            // Return whether or not we were able to move
            return bMoved;
        }

        public void Reset()
        {
            sNodes = new Stack<Node<T>>();
            // Remember that upon Reset, an enumerator should be *before*
            // the first item. Since the stack is the list of items to visit in
            // the future, if we put the root on the stack that means we're before
            // the first item:
            if (tree.nRoot != null)
            {
                sNodes.Push(tree.nRoot);
            }
        }
    }

    class BreadthFirstEnumerator : IEnumerator<T>
    {
        // Store a reference to the tree we're enumerating:
        protected BST<T> tree;
        // Keep track of the current node inside the tree we're looking at:
        protected Node<T> nCurrent = null;
        // Keep track of future nodes to visit. Because I want to do this in a 
        // First In First Out manner, that means we want a queue:
        protected Queue<Node<T>> qNodes;

        public T Current => nCurrent.Data;

        object IEnumerator.Current => Current;

        public BreadthFirstEnumerator(BST<T> tree)
        {
            this.tree = tree;
            Reset();
        }

        public void Dispose()
        {
            tree = null;
            nCurrent = null;
            qNodes = null;
        }

        public bool MoveNext()
        {
            // Keep track of if we were able to move:
            bool bMoved = false;
            // If there's anything left in the queue (queue has a Count property we can check)
            if (qNodes.Count > 0)
            {
                // That means we *can* move to the next item
                bMoved = true;
                //     We'll grab the next node from the queue as our new current
                nCurrent = qNodes.Dequeue();
                //     If that node has any children, we'll add them to the queue
                //       First left, right
                if (nCurrent.Left != null)
                {
                    qNodes.Enqueue(nCurrent.Left);
                }
                if (nCurrent.Right != null)
                {
                    qNodes.Enqueue(nCurrent.Right);
                }
            }

            // Return whether or not we were able to move
            return bMoved;
        }

        public void Reset()
        {
            qNodes = new Queue<Node<T>>();
            // Remember that upon Reset, an enumerator should be *before*
            // the first item. Since the queue is the list of items to visit in
            // the future, if we put the root on the stack that means we're before
            // the first item:
            if (tree.nRoot != null)
            {
                qNodes.Enqueue(tree.nRoot);
            }
        }
    }
}
