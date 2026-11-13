using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BinarySearchTrees;

namespace TreeTests;

public class NodeTests
{
    [Test]
    public void TestNode()
    {
        Node<string> root = new Node<string>("Root");
        root.Left = new Node<string>("Left");
        root.Right = new Node<string>("Right");
        root.Right.Left = new Node<string>("RightLeft");
        root.Right.Right = new Node<string>("RightRight");
        Assert.IsFalse(root.IsLeaf());
        Assert.IsFalse(root.Right.IsLeaf());
        Assert.IsTrue(root.Left.IsLeaf());
        string treeString = "(Root L((Left)) R((Right L((RightLeft)) R((RightRight)))))";
        Assert.AreEqual(treeString, root.ToString());
    }
}

public class BSTTests
{
    private BST<int> iBST;
    private BST<Student> stuBST;

    [SetUp]
    public void BSTTestSetup()
    {
        iBST = new BST<int>();
        iBST.Add(50);
        iBST.Add(20);
        iBST.Add(27);
        iBST.Add(77);
        iBST.Add(22);
        iBST.Add(90);
        iBST.Add(89);
        iBST.Add(91);
        /*
         *       50
         *     /     \
         *    20      77
         *      \       \
         *      27      90
         *     /       /  \
         *     22     89   91
         *
         */
        stuBST = new BST<Student>();
        stuBST.Add(new Student("Mandy", 501, "Mandy@hotmail.com"));
        stuBST.Add(new Student("Hu", 133, "hu@gmail.com"));
        stuBST.Add(new Student("Nancy", 613, "nancy@saskpolytech.ca"));
        stuBST.Add(new Student("Ahmed", 189, "ahmed@gmail.com"));
        stuBST.Add(new Student("Jane", 648, "jane@outlook.com"));
        /*                  Mandy
         *                /      \
         *               Hu      Nancy
         *                 \         \
         *                 Ahmed    Jane
         */       
    }

    [Test]
    public void TestIterator()
    {
        string traversalString = "";
        // Test Pre-order:
        // Visit the root
        // Iterate on left subtree
        // Iterate on right subtree:
        iBST.Iterate(AddToString, TRAVERSALORDER.PRE_ORDER);
        Assert.AreEqual("50-20-27-22-77-90-89-91-", traversalString);

        // In-Order:
        // Iterate on left subtree
        // Visit on the root
        // Iterate on the right subtree
        traversalString = "";
        iBST.Iterate(AddToString, TRAVERSALORDER.IN_ORDER);
        Assert.AreEqual("20-22-27-50-77-89-90-91-", traversalString);

        // Post-Order:
        // Iterate on left subtree
        // Iterate on the right subtree
        // Visit the root
        traversalString = "";
        iBST.Iterate(AddToString, TRAVERSALORDER.POST_ORDER);
        Assert.AreEqual("22-27-20-89-91-90-77-50-", traversalString);

        void AddToString(int i)
        {
            traversalString += i.ToString() + "-";
        }

    }

    [Test]
    public void TestBSTDepthFirstEnumerator()
    {
        // Default enumerator is Depth first:
        IEnumerator<int> myEnum = iBST.GetEnumerator();
        myEnum.Reset();
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(50, myEnum.Current);
        // We'll assume left child first
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(20, myEnum.Current);
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(27, myEnum.Current);
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(22, myEnum.Current);
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(77, myEnum.Current);
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(90, myEnum.Current);
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(89, myEnum.Current);
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(91, myEnum.Current);
        // We've enumerated the whole tree, so MoveNext should return false:
        Assert.IsFalse(myEnum.MoveNext());
        Assert.AreEqual(91, myEnum.Current);
        // May as well dispose at the end:
        myEnum.Dispose();
    }

    [Test]
    public void TestBSTBreadthFirstEnumerator()
    {
        // Grab the Breadth First Enumerator:
        IEnumerator<int> myEnum = iBST.GetBreadthFirstEnumerator();
        myEnum.Reset();
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(50, myEnum.Current);
        // Assume left child is visited first
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(20, myEnum.Current);
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(77, myEnum.Current);
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(27, myEnum.Current);
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(90, myEnum.Current);
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(22, myEnum.Current);
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(89, myEnum.Current);
        Assert.IsTrue(myEnum.MoveNext());
        Assert.AreEqual(91, myEnum.Current);
        // We've enumerated the whole tree, so MoveNext should return false:
        Assert.IsFalse(myEnum.MoveNext());
        // We should remain on the last visited element:
        Assert.AreEqual(91, myEnum.Current);
        // Finally, use the Dispose:
        myEnum.Dispose();
    }

    [Test]
    public void TestBSTRemove()
    {
        Assert.IsFalse(iBST.Remove(9999));
        Assert.AreEqual(8, iBST.Count);
        Assert.IsTrue(iBST.Remove(91));
        Assert.AreEqual(7, iBST.Count);
        Assert.Throws<ApplicationException>(() => iBST.Find(91));
        Assert.AreEqual(90, iBST.Find(90));
        Assert.IsTrue(iBST.Remove(50));
        Assert.AreEqual(6, iBST.Count);
        Assert.Throws<ApplicationException>(() => iBST.Find(50));
        Assert.AreEqual(20, iBST.Find(20));
        Assert.AreEqual(77, iBST.Find(77));
    }

    [Test]
    public void TestBSTFindLargest()
    {
        Assert.AreEqual(91, iBST.FindLargest());
        Assert.AreEqual("Jane", stuBST.FindLargest().Name);
        BST<string> emptyBST = new BST<string>();
        Assert.Throws<ApplicationException>(()  => emptyBST.FindLargest());
    }

    [Test]
    public void TestBSTFindSmallest()
    {
        Assert.AreEqual(20, iBST.FindSmallest());
        Assert.AreEqual("Hu", stuBST.FindSmallest().Name);
        BST<string> emptyBST = new BST<string>();
        Assert.Throws<ApplicationException>(() => emptyBST.FindSmallest());
    }

    [Test]
    public void TestBSTFind()
    {
        Student jane = stuBST.Find(new Student(null, 648, null));
        Assert.AreEqual("Jane", jane.Name);
        Assert.AreEqual(648, jane.StudentNumber);
        Assert.AreEqual("jane@outlook.com", jane.Email);

        // If we try to find something that isn't in the tree,
        // we should get an ApplicationException:
        Assert.Throws<ApplicationException>(() => stuBST.Find(new Student(null, 999, null)));

        Assert.AreEqual(50, iBST.Find(50));
        Assert.AreEqual(90, iBST.Find(90));
        Assert.AreEqual(22, iBST.Find(22));
        Assert.AreEqual(91, iBST.Find(91));
        Assert.Throws<ApplicationException>(() => iBST.Find(1000));
    }

    [Test]
    public void TestHeight()
    {
        Assert.AreEqual(3, iBST.Height());
        BST<string> sBST = new BST<string>();
        Assert.AreEqual(-1, sBST.Height());
        sBST.Add("A");
        sBST.Add("B");
        sBST.Add("C");
        sBST.Add("D");
        sBST.Add("E");
        Assert.AreEqual(4, sBST.Height());
    }

    [Test]
    public void TestAdd()
    {
        BST<string> bst = new BST<string>();
        Assert.AreEqual(0, bst.Count);
        bst.Add("Root");
        Assert.AreEqual(1, bst.Count);
        bst.Add("ALeft");
        bst.Add("ZRightN");
        bst.Add("ZRightA");
        bst.Add("ZRightZ");
        Assert.AreEqual(5, bst.Count);
        string treeString = "Tree: (Root L((ALeft)) R((ZRightN L((ZRightA)) R((ZRightZ)))))";
        Assert.AreEqual(treeString, bst.ToString());
        Console.WriteLine(treeString);
    }
}

public class AVLTTests
{
    private AVLT<int> iAVLT;
    private AVLT<Student> stuAVLT;

    [SetUp]
    public void BSTTestSetup()
    {
        iAVLT = new AVLT<int>();
        iAVLT.Add(50);
        iAVLT.Add(20);
        iAVLT.Add(27);
        iAVLT.Add(77);
        iAVLT.Add(22);
        iAVLT.Add(90);
        iAVLT.Add(89);
        iAVLT.Add(91);

        stuAVLT = new AVLT<Student>();
        stuAVLT.Add(new Student("Mandy", 501, "Mandy@hotmail.com"));
        stuAVLT.Add(new Student("Hu", 133, "hu@gmail.com"));
        stuAVLT.Add(new Student("Nancy", 613, "nancy@saskpolytech.ca"));
        stuAVLT.Add(new Student("Ahmed", 189, "ahmed@gmail.com"));
        stuAVLT.Add(new Student("Jane", 648, "jane@outlook.com"));
    }

    [Test]
    public void TestIterator()
    {

        // In-Order:
        // Iterate on left subtree
        // Visit on the root
        // Iterate on the right subtree
        string traversalString = "";
        iAVLT.Iterate(AddToString, TRAVERSALORDER.IN_ORDER);
        Assert.AreEqual("20-22-27-50-77-89-90-91-", traversalString);

        void AddToString(int i)
        {
            traversalString += i.ToString() + "-";
        }

    }

    [Test]
    public void TestBSTRemove()
    {
        Assert.IsFalse(iAVLT.Remove(9999));
        Assert.AreEqual(8, iAVLT.Count);
        Assert.IsTrue(iAVLT.Remove(91));
        Assert.AreEqual(7, iAVLT.Count);
        Assert.Throws<ApplicationException>(() => iAVLT.Find(91));
        Assert.AreEqual(90, iAVLT.Find(90));
        Assert.IsTrue(iAVLT.Remove(50));
        Assert.AreEqual(6, iAVLT.Count);
        Assert.Throws<ApplicationException>(() => iAVLT.Find(50));
        Assert.AreEqual(20, iAVLT.Find(20));
        Assert.AreEqual(77, iAVLT.Find(77));
    }

    [Test]
    public void TestBSTFindLargest()
    {
        Assert.AreEqual(91, iAVLT.FindLargest());
        Assert.AreEqual("Jane", stuAVLT.FindLargest().Name);
        BST<string> emptyBST = new BST<string>();
        Assert.Throws<ApplicationException>(() => emptyBST.FindLargest());
    }

    [Test]
    public void TestBSTFindSmallest()
    {
        Assert.AreEqual(20, iAVLT.FindSmallest());
        Assert.AreEqual("Hu", stuAVLT.FindSmallest().Name);
        BST<string> emptyBST = new BST<string>();
        Assert.Throws<ApplicationException>(() => emptyBST.FindSmallest());
    }

    [Test]
    public void TestBSTFind()
    {
        Student jane = stuAVLT.Find(new Student(null, 648, null));
        Assert.AreEqual("Jane", jane.Name);
        Assert.AreEqual(648, jane.StudentNumber);
        Assert.AreEqual("jane@outlook.com", jane.Email);

        // If we try to find something that isn't in the tree,
        // we should get an ApplicationException:
        Assert.Throws<ApplicationException>(() => stuAVLT.Find(new Student(null, 999, null)));

        Assert.AreEqual(50, iAVLT.Find(50));
        Assert.AreEqual(90, iAVLT.Find(90));
        Assert.AreEqual(22, iAVLT.Find(22));
        Assert.AreEqual(91, iAVLT.Find(91));
        Assert.Throws<ApplicationException>(() => iAVLT.Find(1000));
    }

    [Test]
    public void TestHeight()
    {
        Assert.AreEqual(3, iAVLT.Height());
        BST<string> sBST = new BST<string>();
        Assert.AreEqual(-1, sBST.Height());
        sBST.Add("A");
        sBST.Add("B");
        sBST.Add("C");
        sBST.Add("D");
        sBST.Add("E");
        Assert.AreEqual(4, sBST.Height());
    }

    [Test]
    public void TestAdd()
    {
        BST<string> bst = new BST<string>();
        Assert.AreEqual(0, bst.Count);
        bst.Add("Root");
        Assert.AreEqual(1, bst.Count);
        bst.Add("ALeft");
        bst.Add("ZRightN");
        bst.Add("ZRightA");
        bst.Add("ZRightZ");
        Assert.AreEqual(5, bst.Count);
        string treeString = "Tree: (Root L((ALeft)) R((ZRightN L((ZRightA)) R((ZRightZ)))))";
        Assert.AreEqual(treeString, bst.ToString());
        Console.WriteLine(treeString);
    }

    [Test]
    public void TestSingleLeft()
    {
        Node<string> root = new Node<string>("OldRoot");
        root.Left = new Node<string>("OldLeft");
        root.Right = new Node<string>("OldRight");
        root.Left.Left = new Node<string>("OldLeftLeft");
        root.Left.Right = new Node<string>("OldLeftRight");
        root.Right.Left = new Node<string>("OldRightLeft");
        root.Right.Right = new Node<string>("OldRightRight");
        root = AVLT<string>.SingleLeft(root);
        Assert.AreEqual("OldRight", root.Data);
        Assert.AreEqual("OldRoot", root.Left.Data);
        Assert.AreEqual("OldRightRight", root.Right.Data);
        Assert.AreEqual("OldRightLeft", root.Left.Right.Data);
    }

    [Test]
    public void TestSingleRight()
    {
        Node<string> root = new Node<string>("OldRoot");
        root.Left = new Node<string>("OldLeft");
        root.Right = new Node<string>("OldRight");
        root.Left.Left = new Node<string>("OldLeftLeft");
        root.Left.Right = new Node<string>("OldLeftRight");
        root.Right.Left = new Node<string>("OldRightLeft");
        root.Right.Right = new Node<string>("OldRightRight");
        root = AVLT<string>.SingleRight(root);
        Assert.AreEqual("OldLeft", root.Data);
        Assert.AreEqual("OldRoot", root.Right.Data);
        Assert.AreEqual("OldLeftLeft", root.Left.Data);
        Assert.AreEqual("OldLeftRight", root.Right.Left.Data);
    }

    [Test]
    public void TestDoubleLeft()
    {
        Node<string> root = new Node<string>("OldRoot");
        root.Left = new Node<string>("OldLeft");
        root.Right = new Node<string>("OldRight");
        root.Left.Left = new Node<string>("OldLeftLeft");
        root.Left.Right = new Node<string>("OldLeftRight");
        root.Right.Left = new Node<string>("OldRightLeft");
        root.Right.Right = new Node<string>("OldRightRight");
        root = AVLT<string>.DoubleLeft(root);
        Assert.AreEqual("OldRightLeft", root.Data);
        Assert.AreEqual("OldRoot", root.Left.Data);
        Assert.AreEqual("OldRight", root.Right.Data);
    }

    [Test]
    public void TestDoubleRight()
    {
        Node<string> root = new Node<string>("OldRoot");
        root.Left = new Node<string>("OldLeft");
        root.Right = new Node<string>("OldRight");
        root.Left.Left = new Node<string>("OldLeftLeft");
        root.Left.Right = new Node<string>("OldLeftRight");
        root.Right.Left = new Node<string>("OldRightLeft");
        root.Right.Right = new Node<string>("OldRightRight");
        root = AVLT<string>.DoubleRight(root);
        Assert.AreEqual("OldLeftRight", root.Data);
        Assert.AreEqual("OldRoot", root.Right.Data);
        Assert.AreEqual("OldLeft", root.Left.Data);
    }

    [Test]
    public void TestGetHeightDifference()
    {
        Node<string> root = new Node<string>("Root");
        Assert.AreEqual(0, AVLT<string>.GetHeightDifference(root));
        root.Right = new Node<string>("Right");
        Assert.AreEqual(1, AVLT<string>.GetHeightDifference(root));
        root.Left = new Node<string>("Left");
        Assert.AreEqual(0, AVLT<string>.GetHeightDifference(root));
        root.Left.Left = new Node<string>("LeftLeft");
        Assert.AreEqual(-1, AVLT<string>.GetHeightDifference(root));
        root.Left.Left.Right = new Node<string>("LeftLeftRight");
        Assert.AreEqual(-2, AVLT<string>.GetHeightDifference(root));
    }

    [Test]
    public void TestAVLTBalancing()
    {
        AVLT<int> avlt = new AVLT<int>();
        BST<int> bst = new BST<int>();

        bst.Add(100);
        avlt.Add(100);
        bst.Add(125);
        avlt.Add(125);
        bst.Add(110);
        avlt.Add(110);
        Assert.AreEqual(2, bst.Height());
        Assert.AreEqual(1, avlt.Height());
        foreach (int i in bst)
        {
            Assert.IsTrue(avlt.Contains(i));
        }

        bst.Add(90);
        avlt.Add(90);
        bst.Add(105);
        avlt.Add(105);
        bst.Add(107);
        avlt.Add(107);
        Assert.AreEqual(4, bst.Height());
        Assert.AreEqual(2, avlt.Height());
        foreach (int i in bst)
        {
            Assert.IsTrue(avlt.Contains(i));
        }

        bst.Add(130);
        avlt.Add(130);
        bst.Add(150);
        avlt.Add(150);
        Assert.AreEqual(4, bst.Height());
        Assert.AreEqual(3, avlt.Height());
        foreach (int i in bst)
        {
            Assert.IsTrue(avlt.Contains(i));
        }
    }
}

public class TreeSpeedTests
{
    BST<int> iBST;
    AVLT<int> iAVLT;

    [SetUp]
    public void Setup()
    {
        iBST = new BST<int>();
        iAVLT = new AVLT<int>();
    }

    // Time how long it takes to add nums to the tree
    private long timeTreeAdd(A_BST<int> tree, int[] nums)
    {
        long start = Environment.TickCount;
        foreach (int i in nums)
        {
            tree.Add(i);
        }
        return Environment.TickCount - start;
    }

    // Time how long it takes to find nums in the tree
    private long timeTreeFind(A_BST<int> tree, int[] nums)
    {
        long start = Environment.TickCount;
        foreach (int i in nums)
        {
            tree.Find(i);
        }
        return Environment.TickCount - start;
    }

    [Test]
    public void TreeSpeedRandomComparison()
    {
        // How many numbers to generate?
        int numInts = 50000;
        Random rnd = new Random(9999);
        int[] numbers = new int[numInts];
        for (int i = 0; i < numInts; i++)
        {
            numbers[i] = rnd.Next(0, 1000000);
        }

        Console.WriteLine("Adding {0} random ints...", numInts);
        Console.WriteLine("The BST took {0} ms to add, and ended up height {1}",
            timeTreeAdd(iBST, numbers), iBST.Height());
        Console.WriteLine("The AVLT took {0} ms to add, and ended up height {1}",
            timeTreeAdd(iAVLT, numbers), iAVLT.Height());
        Console.WriteLine("Finding {0} items took the BST {1} ms.",
            numInts, timeTreeFind(iBST, numbers));
        Console.WriteLine("Finding {0} items took the AVLT {1} ms.",
            numInts, timeTreeFind(iAVLT, numbers));
    }

    [Test]
    public void TreeSpeedSequentialComparison()
    {
        // How many numbers to generate?
        int numInts = 1000;
        int[] numbers = new int[numInts];
        for (int i = 0; i < numInts; i++)
        {
            numbers[i] = i;
        }

        Console.WriteLine("Adding {0} sequential ints...", numInts);
        Console.WriteLine("The BST took {0} ms to add, and ended up height {1}",
            timeTreeAdd(iBST, numbers), iBST.Height());
        Console.WriteLine("The AVLT took {0} ms to add, and ended up height {1}",
            timeTreeAdd(iAVLT, numbers), iAVLT.Height());
        Console.WriteLine("Finding {0} items took the BST {1} ms.",
            numInts, timeTreeFind(iBST, numbers));
        Console.WriteLine("Finding {0} items took the AVLT {1} ms.",
            numInts, timeTreeFind(iAVLT, numbers));
    }

    [Test]
    public void TreeSpeedSemiRandomIncreasingComparison()
    {
        // How many numbers to generate?
        int numInts = 50000;
        Random rnd = new Random(9999);
        int[] numbers = new int[numInts];
        for (int i = 0; i < numInts; i++)
        {
            numbers[i] = i + rnd.Next(-500, 500);
        }

        Console.WriteLine("Adding {0} semi-random increasing ints...", numInts);
        Console.WriteLine("The BST took {0} ms to add, and ended up height {1}",
            timeTreeAdd(iBST, numbers), iBST.Height());
        Console.WriteLine("The AVLT took {0} ms to add, and ended up height {1}",
            timeTreeAdd(iAVLT, numbers), iAVLT.Height());
        Console.WriteLine("Finding {0} items took the BST {1} ms.",
            numInts, timeTreeFind(iBST, numbers));
        Console.WriteLine("Finding {0} items took the AVLT {1} ms.",
            numInts, timeTreeFind(iAVLT, numbers));
    }
}
