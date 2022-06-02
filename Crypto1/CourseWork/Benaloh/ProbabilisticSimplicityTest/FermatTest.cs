using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CourseWork.Benaloh.Stuff;

namespace CourseWork.Benaloh.ProbabilisticSimplicityTest
{
    public class FermatTest : IProbabilisticSimplicityTest
    {
        public Boolean MakeSimplicityTest(BigInteger value, Double minProbability)
        {
            if (minProbability is < 0.5 or >= 1)
            {
                throw new ArgumentException(nameof(minProbability));
            }
            
            if (value == 1)
            {
                return false;
            }

            var randomNumbers = new HashSet<BigInteger>();

            for (var i = 0; 1.0 - Math.Pow(2, -i) <= minProbability; i++)
            {
                while (randomNumbers.Count <= i)
                {
                    var randomNumber = Utils.RandomBigInteger(2, value - 1);
                    if (!randomNumbers.Contains(randomNumber))
                    {
                        randomNumbers.Add(randomNumber);
                    }
                }
                if (BigInteger.ModPow(randomNumbers.Last(), value - 1, value) != 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
