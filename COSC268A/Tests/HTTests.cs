using HashTables;
using Lists;
using System;
using System.Collections.Generic;
using System.IO;

namespace HTTests;

public class PersonTests
{
    [Test]
    public void TestGetHashCode()
    {
        int[] sins = { 232382319, 293820211, 969482132, 939555444 };
        foreach (int i in sins)
        {
            Person p = new Person(i, "", "", "");
            Console.WriteLine("SIN: {0} == Hashcode: {1}", i, p.GetHashCode());
        }
    }
}

public class ChainingHTTests
{
    [Test]
    public void TestAddAndGetAndClear()
    {
        ChainingHT<int, string> intStringHT = new ChainingHT<int, string>();
        Assert.AreEqual(0, intStringHT.Count);

        intStringHT.Add(0, "Zero");
        intStringHT.Add(1, "One");
        intStringHT.Add(2, "Two");
        intStringHT.Add(3, "Three");
        intStringHT.Add(4, "Four");
        Assert.AreEqual(5, intStringHT.Count);

        Assert.AreEqual("Zero", intStringHT.Get(0));
        Assert.AreEqual("One", intStringHT.Get(1));
        Assert.AreEqual("Two", intStringHT.Get(2));
        Assert.AreEqual("Three", intStringHT.Get(3));
        Assert.AreEqual("Four", intStringHT.Get(4));

        Assert.Throws<ApplicationException>(() => intStringHT.Get(9999));

        intStringHT.Clear();
        Assert.AreEqual(0, intStringHT.Count);
        Assert.Throws<ApplicationException>(() => intStringHT.Get(1));
    }

    [Test]
    public void TestRemove()
    {
        ChainingHT<int, string> intStringHT = new ChainingHT<int, string>();
        Assert.AreEqual(0, intStringHT.Count);

        intStringHT.Add(0, "Zero");
        intStringHT.Add(1, "One");
        intStringHT.Add(2, "Two");
        intStringHT.Add(3, "Three");
        intStringHT.Add(4, "Four");
        Assert.AreEqual(5, intStringHT.Count);

        Assert.AreEqual("Four", intStringHT.Remove(4));
        Assert.AreEqual(4, intStringHT.Count);
        Assert.AreEqual("Zero", intStringHT.Remove(0));
        Assert.AreEqual(3, intStringHT.Count);
        Assert.Throws<ApplicationException>(() => intStringHT.Remove(9999));

        Assert.AreEqual("One", intStringHT.Get(1));
        Assert.AreEqual("Two", intStringHT.Get(2));
        Assert.AreEqual("Three", intStringHT.Get(3));
        Assert.Throws<ApplicationException>(() => intStringHT.Get(0));
        Assert.Throws<ApplicationException>(() => intStringHT.Get(4));
    }

    [Test]
    public void TestEnumerator()
    {
        ChainingHT<int, Person> cht = new ChainingHT<int, Person>();
        Array_List<Person> alPeople = new Array_List<Person>();
        alPeople.Add(new Person(111, "Ada", "Ace", "ada.ace@gmail.com"));
        alPeople.Add(new Person(222, "Bill", "Bee", "bill.bee@gmail.com"));
        alPeople.Add(new Person(333, "Cat", "Cee", "cat.cee@gmail.com"));
        alPeople.Add(new Person(444, "Dirk", "Dee", "dirk.dee@outlook.com"));
        alPeople.Add(new Person(555, "Ed", "Eee", "ed.eee@saskpolytech.ca"));
        alPeople.Add(new Person(666, "Fred", "Fee", "Fred.Fee@gmail.com"));

        Array_List<int> alSINS = new Array_List<int>();
        foreach (Person person in alPeople)
        {
            cht.Add(person.SIN, person);
            alSINS.Add(person.SIN);
        }

        // Now check the enumerator:
        IEnumerator<Person> myEnum = cht.GetEnumerator();
        myEnum.Reset();
        while (myEnum.MoveNext())
        {
            Person p = myEnum.Current;
            // Remove it from our list of SINS - it should be in the list!
            Assert.IsTrue(alSINS.Remove(p.SIN));
        }

        // Once we're done, we should have found every SIN:
        Assert.AreEqual(0, alSINS.Count);
    }
}

public class ChainingBSTTests
{
    [Test]
    public void TestAddAndGetAndClear()
    {
        ChainingBST<int, string> intStringHT = new ChainingBST<int, string>();
        Assert.AreEqual(0, intStringHT.Count);

        intStringHT.Add(0, "Zero");
        intStringHT.Add(1, "One");
        intStringHT.Add(2, "Two");
        intStringHT.Add(3, "Three");
        intStringHT.Add(4, "Four");
        Assert.AreEqual(5, intStringHT.Count);

        Assert.AreEqual("Zero", intStringHT.Get(0));
        Assert.AreEqual("One", intStringHT.Get(1));
        Assert.AreEqual("Two", intStringHT.Get(2));
        Assert.AreEqual("Three", intStringHT.Get(3));
        Assert.AreEqual("Four", intStringHT.Get(4));

        Assert.Throws<ApplicationException>(() => intStringHT.Get(9999));

        intStringHT.Clear();
        Assert.AreEqual(0, intStringHT.Count);
        Assert.Throws<ApplicationException>(() => intStringHT.Get(1));
    }

    [Test]
    public void TestRemove()
    {
        ChainingBST<int, string> intStringHT = new ChainingBST<int, string>();
        Assert.AreEqual(0, intStringHT.Count);

        intStringHT.Add(0, "Zero");
        intStringHT.Add(1, "One");
        intStringHT.Add(2, "Two");
        intStringHT.Add(3, "Three");
        intStringHT.Add(4, "Four");
        Assert.AreEqual(5, intStringHT.Count);

        Assert.AreEqual("Four", intStringHT.Remove(4));
        Assert.AreEqual(4, intStringHT.Count);
        Assert.AreEqual("Zero", intStringHT.Remove(0));
        Assert.AreEqual(3, intStringHT.Count);
        Assert.Throws<ApplicationException>(() => intStringHT.Remove(9999));

        Assert.AreEqual("One", intStringHT.Get(1));
        Assert.AreEqual("Two", intStringHT.Get(2));
        Assert.AreEqual("Three", intStringHT.Get(3));
        Assert.Throws<ApplicationException>(() => intStringHT.Get(0));
        Assert.Throws<ApplicationException>(() => intStringHT.Get(4));
    }

    [Test]
    public void TestEnumerator()
    {
        ChainingBST<int, Person> cht = new ChainingBST<int, Person>();
        Array_List<Person> alPeople = new Array_List<Person>();
        alPeople.Add(new Person(111, "Ada", "Ace", "ada.ace@gmail.com"));
        alPeople.Add(new Person(222, "Bill", "Bee", "bill.bee@gmail.com"));
        alPeople.Add(new Person(333, "Cat", "Cee", "cat.cee@gmail.com"));
        alPeople.Add(new Person(444, "Dirk", "Dee", "dirk.dee@outlook.com"));
        alPeople.Add(new Person(555, "Ed", "Eee", "ed.eee@saskpolytech.ca"));
        alPeople.Add(new Person(666, "Fred", "Fee", "Fred.Fee@gmail.com"));

        Array_List<int> alSINS = new Array_List<int>();
        foreach (Person person in alPeople)
        {
            cht.Add(person.SIN, person);
            alSINS.Add(person.SIN);
        }

        // Now check the enumerator:
        IEnumerator<Person> myEnum = cht.GetEnumerator();
        myEnum.Reset();
        while (myEnum.MoveNext())
        {
            Person p = myEnum.Current;
            // Remove it from our list of SINS - it should be in the list!
            Assert.IsTrue(alSINS.Remove(p.SIN));
        }

        // Once we're done, we should have found every SIN:
        Assert.AreEqual(0, alSINS.Count);
    }
}

public class LinearHTTests
{
    [Test]
    public void TestAddAndGetAndClear()
    {
        LinearHT<int, string> intStringHT = new LinearHT<int, string>();
        Assert.AreEqual(0, intStringHT.Count);

        intStringHT.Add(0, "Zero");
        intStringHT.Add(1, "One");
        intStringHT.Add(2, "Two");
        intStringHT.Add(3, "Three");
        intStringHT.Add(4, "Four");
        Assert.AreEqual(5, intStringHT.Count);

        Assert.AreEqual("Zero", intStringHT.Get(0));
        Assert.AreEqual("One", intStringHT.Get(1));
        Assert.AreEqual("Two", intStringHT.Get(2));
        Assert.AreEqual("Three", intStringHT.Get(3));
        Assert.AreEqual("Four", intStringHT.Get(4));

        Assert.Throws<ApplicationException>(() => intStringHT.Get(9999));

        intStringHT.Clear();
        Assert.AreEqual(0, intStringHT.Count);
        Assert.Throws<ApplicationException>(() => intStringHT.Get(1));
    }

    [Test]
    public void TestRemove()
    {
        LinearHT<int, string> intStringHT = new LinearHT<int, string>();
        Assert.AreEqual(0, intStringHT.Count);

        intStringHT.Add(0, "Zero");
        intStringHT.Add(1, "One");
        intStringHT.Add(2, "Two");
        intStringHT.Add(3, "Three");
        intStringHT.Add(4, "Four");
        Assert.AreEqual(5, intStringHT.Count);

        Assert.AreEqual("Four", intStringHT.Remove(4));
        Assert.AreEqual(4, intStringHT.Count);
        Assert.AreEqual("Zero", intStringHT.Remove(0));
        Assert.AreEqual(3, intStringHT.Count);
        Assert.Throws<ApplicationException>(() => intStringHT.Remove(9999));

        Assert.AreEqual("One", intStringHT.Get(1));
        Assert.AreEqual("Two", intStringHT.Get(2));
        Assert.AreEqual("Three", intStringHT.Get(3));
        Assert.Throws<ApplicationException>(() => intStringHT.Get(0));
        Assert.Throws<ApplicationException>(() => intStringHT.Get(4));
    }

    [Test]
    public void TestEnumerator()
    {
        LinearHT<int, Person> cht = new LinearHT<int, Person>();
        Array_List<Person> alPeople = new Array_List<Person>();
        alPeople.Add(new Person(111, "Ada", "Ace", "ada.ace@gmail.com"));
        alPeople.Add(new Person(222, "Bill", "Bee", "bill.bee@gmail.com"));
        alPeople.Add(new Person(333, "Cat", "Cee", "cat.cee@gmail.com"));
        alPeople.Add(new Person(444, "Dirk", "Dee", "dirk.dee@outlook.com"));
        alPeople.Add(new Person(555, "Ed", "Eee", "ed.eee@saskpolytech.ca"));
        alPeople.Add(new Person(666, "Fred", "Fee", "Fred.Fee@gmail.com"));

        Array_List<int> alSINS = new Array_List<int>();
        foreach (Person person in alPeople)
        {
            cht.Add(person.SIN, person);
            alSINS.Add(person.SIN);
        }

        // Now check the enumerator:
        IEnumerator<Person> myEnum = cht.GetEnumerator();
        myEnum.Reset();
        while (myEnum.MoveNext())
        {
            Person p = myEnum.Current;
            // Remove it from our list of SINS - it should be in the list!
            Assert.IsTrue(alSINS.Remove(p.SIN));
        }

        // Once we're done, we should have found every SIN:
        Assert.AreEqual(0, alSINS.Count);
    }
}

public class HashTableSpeedTests
{

    Array_List<Person> peopleAL =
        loadPeopleFromFile(@"C:\Users\Dustin School\Downloads\tenthousandpeople.csv");

    ChainingHT<Person, Person> personCHT;

    [SetUp]
    public void Setup()
    {
        personCHT = new ChainingHT<Person, Person>();
    }

    [Test]
    public void TestDoubleHTAddAndGetSpeed()
    {
        DoubleHT<Person, Person> personDHT = new DoubleHT<Person, Person>();
        addLotsOfPeopleToHT(personDHT);
        getLotsOfPeopleToHT(personDHT);
    }

    [Test]
    public void TestQuadraticHTAddAndGetSpeed()
    {
        QuadraticHT<Person, Person> personQHT = new QuadraticHT<Person, Person>();
        addLotsOfPeopleToHT(personQHT);
        getLotsOfPeopleToHT(personQHT);
    }

    [Test]
    public void TestChainingHTAddAndGetSpeed()
    {
        addLotsOfPeopleToHT(personCHT);
        getLotsOfPeopleToHT(personCHT);
    }

    [Test]
    public void TestLinearHTAddAndGetSpeed()
    {
        LinearHT<Person, Person> personLHT = new LinearHT<Person, Person>();
        addLotsOfPeopleToHT(personLHT);
        getLotsOfPeopleToHT(personLHT);
    }

    private void addLotsOfPeopleToHT(A_HashTable<Person, Person> ht)
    {
        int iNumAdded = 0;
        long start = Environment.TickCount;

        foreach (Person p in peopleAL)
        {
            ht.Add(p, p);
            iNumAdded++;
        }

        long totalTime = Environment.TickCount - start;
        Console.WriteLine("Added {0} entries in {1} ms with {2} collisions",
            iNumAdded, totalTime, ht.NumCollisions);
    }

    private void getLotsOfPeopleToHT(A_HashTable<Person, Person> ht)
    {
        int iNumGotten = 0;
        long start = Environment.TickCount;

        foreach (Person p in peopleAL)
        {
            Person personFromHT = ht.Get(p);
            Assert.AreEqual(p, personFromHT);
            iNumGotten++;
        }

        long totalTime = Environment.TickCount - start;
        Console.WriteLine("Got {0} entries in {1} ms.",
            iNumGotten, totalTime);
    }

    [Test]
    public void TestLoadPeopleFromFile()
    {
        Array_List<Person> persons =
            loadPeopleFromFile(@"C:\Users\Dustin School\Downloads\tenthousandpeople.csv");
        foreach (Person person in persons)
        {
            Console.WriteLine(person.ToString());
        }
    }

    private static Array_List<Person> loadPeopleFromFile(string filename)
    {
        // Array List to store our ten thousand people:
        Array_List<Person> alPerson = new Array_List<Person>();

        // An easy way to read in files in C# is to just enumerator over them
        IEnumerable<string> lines = File.ReadLines(filename);
        foreach (string line in lines)
        {
            // String has a split method we can use to split up each line:
            string[] sArray = line.Split(",");
            int iSIN = Int32.Parse(sArray[3]);
            Person p = new Person(iSIN, sArray[0], sArray[1], sArray[2]);
            alPerson.Add(p);
        }
        return alPerson;
    }
}
