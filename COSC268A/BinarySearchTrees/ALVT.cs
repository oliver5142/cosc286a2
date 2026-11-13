using System;
using System.Collections.Generic;

namespace BinarySearchTrees;

public class AVLT<T>: BST<T> where T: IComparable<T>
{
    /// <summary>
    /// Balances the subtree, assuming its subtrees were already balanced
    /// </summary>
    /// <param name="nCurrent">The root node of the subtree being balanced</param>
    /// <returns>The root node of the newly balanced subtree</returns>
    internal override Node<T> Balance(Node<T> nCurrent)
    {
        // Keep track of what the new root is. We'll start off assuming it is nCurrent
        Node<T> nNewRoot = nCurrent;

        // If the current node is null, obviously change nothing
        if (nCurrent != null)
        {
            // Otherwise, we need to find out the balance factor of the current node
            int iHeightDiff = GetHeightDifference(nCurrent);
            // We only need to balance if the balance factor is less than -1 or greater than +1
            if (iHeightDiff > 1)
            {
                // If we are unbalanced to the right (so balance factor 2+)
                // We need to figure out what rotation to do, so we need the balance factor
                // Of our right subtree
                int iRightChildHeightDiff = GetHeightDifference(nCurrent.Right);
                // If the right subtree is left heavy
                if (iRightChildHeightDiff < 0)
                {
                    // We need to do a double left
                    nNewRoot = DoubleLeft(nCurrent);
                }
                else
                {
                    // Otherwise
                    // We need to do a single left
                    nNewRoot = SingleLeft(nCurrent);
                }
            }
            else if (iHeightDiff < -1)
            {
                // If we're unbalanced to the left (so balance factor -2 or less)
                // We need to figure out the rotation, so we need balance factor of left subtree
                int iLeftChildHeightDiff = GetHeightDifference(nCurrent.Left);
                // If the left subtree is right heavy
                if (iLeftChildHeightDiff > 0)
                {
                    // We need to do a double right
                    nNewRoot = DoubleRight(nCurrent);
                }
                else
                {
                    // Otherwise
                    // We need to do a single right
                    nNewRoot = SingleRight(nCurrent);
                }
            }
            // If we aren't imbalanced either direction, we don't need to do anything
        }

        // Return the new root, whatever it may be
        return nNewRoot;
    }

    // Normally this would be private, but I'm going to make it public and static
    // so we can test it:
    /// <summary>
    /// Determine the height difference between the left and right subtree
    /// This will be height on the right minus height on the left
    /// Positive means we're right heavy, negative means we're left heavy
    /// </summary>
    /// <param name="nCurrent">The root node of the subtree to analyze</param>
    /// <returns>The balance factor</returns>
    public static int GetHeightDifference(Node<T> nCurrent)
    {
        int iHeightDiff = 0;
        int iHeightRight = 0;
        int iHeightLeft = 0;
        // Make sure to check nCurrent isn't null
        if (nCurrent != null)
        {
            // Get the heights of the left and right subtree (remember the recHeight method exists)
            // If you don't have a subtree on one side, that side is 0.
            // If you do, it'll be the height of that subtree plus 1

            // Grab the height on the right, if it exists:
            if (nCurrent.Right != null)
            {
                iHeightRight = recHeight(nCurrent.Right) + 1;
            }
            // Grab the height on the left, if it exists:
            if (nCurrent.Left != null)
            {
                iHeightLeft = recHeight(nCurrent.Left) + 1;
            }

            iHeightDiff = iHeightRight - iHeightLeft;
        }

        // Calculate and return the height difference
        return iHeightDiff;
    }

    // Normally I'd write rotation functions as private (they're internal helper functions)
    // but I'll write them as static and public so we can test them and understand them easier

    /// <summary>
    /// Performs a single Left Rotation
    /// Given an original node,
    /// We will make the root of the tree point to the right child of the original root.
    /// We will make the old root's right child equal to the new root's left child.
    /// We will make the new root's left child the old root
    /// </summary>
    /// <param name="nOldRoot">The original root of the tree</param>
    /// <returns>The new root of the tree</returns>
    public static Node<T> SingleLeft(Node<T> nOldRoot)
    {
        // The new root will be the old root's right child
        Node<T> nNewRoot = nOldRoot.Right;
        // We need to clear space on the left of the new root to put the old root,
        // so we'll assign the new root's left child to be a reference to the old root's
        // right child
        nOldRoot.Right = nNewRoot.Left;
        // The new root's left child will become the old root
        nNewRoot.Left = nOldRoot;

        // Return the new root of the tree
        return nNewRoot;
    }

    public static Node<T> SingleRight(Node<T> nOldRoot)
    {
        Node<T> nNewRoot = nOldRoot.Left;
        nOldRoot.Left = nNewRoot.Right;
        nNewRoot.Right = nOldRoot;

        return nNewRoot;
    }

    public static Node<T> DoubleLeft(Node<T> nOldRoot)
    {
        nOldRoot.Right = SingleRight(nOldRoot.Right);
        return SingleLeft(nOldRoot);
    }

    public static Node<T> DoubleRight(Node<T> nOldRoot)
    {
        nOldRoot.Left = SingleLeft(nOldRoot.Left);
        return SingleRight(nOldRoot);
    }
}
