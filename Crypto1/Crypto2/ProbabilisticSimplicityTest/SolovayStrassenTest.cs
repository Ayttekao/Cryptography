﻿using System;
using System.Numerics;
using Crypto2.Stuff;

namespace Crypto2.ProbabilisticSimplicityTest
{
    public class SolovayStrassenTest : IProbabilisticSimplicityTest
    {
        public bool MakeSimplicityTest(BigInteger value, Double minProbability)
        {
            if (value == 1)
                return false;
            for (var i = 0; 1.0 - Math.Pow(2, -i) <= minProbability; i++)
            {
                var a = Utils.RandomBigInteger(2, value - 1);
                if (BigInteger.GreatestCommonDivisor(a, value) > 1)
                {
                    return false;
                }
                if (BigInteger.ModPow(a, (value - 1) / 2, value) != Functions.Jacobi(a, value))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
