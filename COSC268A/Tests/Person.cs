using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTests;

public class Person : IComparable<Person>
{
    int iSIN = 0;
    string sFirstName = "";
    string sLastName = "";
    string sEmail = "";

    public Person(int iSIN, string sFirstName, string sLastName, string sEmail)
    {
        this.iSIN = iSIN;
        this.sFirstName = sFirstName;
        this.sLastName = sLastName;
        this.sEmail = sEmail;
    }

    // We're going to use the SIN as the key
    // It is important if you're using something as a key,
    // to make sure it cannot be changed.
    public int SIN { get { return iSIN; } }
    public string FirstName { get { return sFirstName; } set {sFirstName= value; }}
    public string LastName { get {return sLastName; } set { sLastName= value; }}
    public string Email { get { return sEmail; } set {sEmail= value; }}

    // We will compare on the basis of SINs
    // If two people have the same SIN, we consider them the same person:
    public int CompareTo(Person other)
    {
        return this.SIN.CompareTo(other.SIN);
    }

    public override string ToString()
    {
        return LastName + ", " + FirstName + " (" + SIN + ")";
    }

    // Two people will be equal if their SINs are equal
    public override bool Equals (object obj)
    {
        Person otherPerson = (Person)obj;
        return this.CompareTo(otherPerson) == 0;
    }

    /// <summary>
    /// Only override this method if you know what you're doing!
    /// 
    /// If you know what you're doing, and if you know your program's data,
    /// you might be able to write a better hash function than the default.
    /// 
    /// Remember: a good hashcode function will generate values that represent all
    ///  the possible space in the integer space, which will allow your table's hash
    ///  function to take up all available table space. So a good GetHashCode
    ///  function will have every integer value have an equal chance of being generated.
    ///  
    /// Some hash functions will work well with some keys, but not others
    /// </summary>
    /// <returns>A hashcode</returns>
    public override int GetHashCode()
    {
        // Let's turn the SIN into a string of characters:
        char[] cChars = SIN.ToString().ToCharArray();

        int iHashCode = 0;
        for (int i = 0; i < cChars.Length; i++)
        {
            // We can just loop through, convert each character to an Ascii value
            // and return the sum:
            // This is maybe not a great hashcode for a SIN:
            // Each SIN is 9 digits. Each ascii digit has a value from 48 to 57.
            // That means the sum of the digits is going to be between 9*48 (000000000)
            // to 9 * 57 (999999999) - only 81 possible hashcodes.
            //iHashCode += (int)cChars[i];

            // Maybe we want to make use of our positional data
            // This technique multiplies each ascii value by a power of 9
            // according to its position in the string. For this particular set
            // of data, we get a much better distribution:
            //iHashCode += ((int)cChars[i]) * (int)Math.Pow(9, i);

            // A common general purpose hashing algorithm is to take a prime number
            // multiply it by the existing hashcode, and then add on the ascii
            // value of the current character:
            int iPrime = 31;
            iHashCode = (iPrime * iHashCode) + cChars[i];
        }
        return iHashCode;
    }
}
