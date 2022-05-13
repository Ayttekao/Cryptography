using System;
using System.Numerics;
using Crypto2.Stuff;

namespace Crypto2.ProbabilisticSimplicityTest
{
    public class MillerRabinTest : IProbabilisticSimplicityTest
    {
        public bool MakeSimplicityTest(BigInteger value, Double minProbability)
        {
            var d = value - 1;
            var degree = 0;
            
            if (value == 1)
                return false;
            
            while (d % 2 == 0)
            {
                d /= 2;
                degree += 1;
            }

            for (var i = 0; 1.0 - Math.Pow(4, -i) <= minProbability; i++)
            {
                var a = Utils.RandomBigInteger(2, value - 1);
                var x = BigInteger.ModPow(a, d, value);
                
                if (x == 1 || x == value - 1)
                    continue;
                
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
