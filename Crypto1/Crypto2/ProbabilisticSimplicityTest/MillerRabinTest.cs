using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Crypto2.Stuff;

namespace Crypto2.ProbabilisticSimplicityTest
{
    public class MillerRabinTest : IProbabilisticSimplicityTest
    {
        public Boolean MakeSimplicityTest(BigInteger value, Double minProbability)
        {
            if (minProbability is < 0.5 or >= 1)
            {
                throw new ArgumentException(nameof(minProbability));
            }
            
            var d = value - 1;
            var degree = 0;
            var randomNumbers = new HashSet<BigInteger>();

            if (value == 1)
            {
                return false;
            }
            
            while (d % 2 == 0)
            {
                d /= 2;
                degree += 1;
            }

            for (var i = 0; 1.0 - Math.Pow(4, -i) <= minProbability; i++)
            {
                while (randomNumbers.Count <= i)
                {
                    var randomNumber = Utils.RandomBigInteger(2, value - 1);
                    if (!randomNumbers.Contains(randomNumber))
                    {
                        randomNumbers.Add(randomNumber);
                    }
                }
                var x = BigInteger.ModPow(randomNumbers.Last(), d, value);
                
                if (x == 1)
                {
                    continue;
                }
                
                for (var r = 1; r < degree; r++)
                {
                    x = BigInteger.ModPow(x, 2, value);
                    if (x == 1)
                        return false;
                    if (x == value - 1)
                        break;
                }

                if (x != value - 1)
                    return false;
            }

            return true;
        }
    }
}
