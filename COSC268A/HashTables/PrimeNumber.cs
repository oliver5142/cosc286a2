using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTables;

/// <summary>
/// This class hands out prime numbers, each roughly double the last one
/// </summary>
public class PrimeNumber
{
    // We'll sotre a list of pre-computed prime numbers:
    int[] iPrimes = { 5, 11, 19, 41, 79, 163, 317, 641, 1_201, 2_399, 4_801,
        9_733, 50_021, 100_003, 200_003, 400_009, 800_011, 1_600_033 };

    // We'llkeep track of which prime number we gave out last, by index:
    int iCurrent = -1;

    // Give out the next prime number:
    public int GetNextPrime()
    {
        iCurrent++;
        if (iCurrent >= iPrimes.Length)
        {
            throw new ApplicationException("Maximum table size exceeded");
        }

        return iPrimes[iCurrent];
    }
}
