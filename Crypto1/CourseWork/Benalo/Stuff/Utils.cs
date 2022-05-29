using System;
using System.Numerics;
using System.Security.Cryptography;

namespace CourseWork.Benalo.Stuff
{
    public static class Utils
    {
        public static BigInteger RandomBigInteger(BigInteger below, BigInteger above)
        {
            BigInteger randomBigInteger;
            var numberGenerator = RandomNumberGenerator.Create();
            var bytes = above.ToByteArray();

            do
            {
                numberGenerator.GetBytes(bytes);
                randomBigInteger = new BigInteger(bytes);
            } while (!(randomBigInteger >= below && randomBigInteger <= above));

            return randomBigInteger;
        }

        public static BigInteger EuclideanAlgorithm(BigInteger m, BigInteger n)
        {
            while (m != 0 && n != 0)
            {
                if (m > n)
                    m %= n;
                else
                    n %= m;
            }
            return m + n;
        }
        public static BigInteger ExtendedEuclideanAlgorithm(BigInteger a,
            BigInteger b,
            out BigInteger x,
            out BigInteger y)
        {
            if (b == 0)
            {
                x = 1;
                y = 0;
                return a;
            }

            var gcd = ExtendedEuclideanAlgorithm(b, a % b, out var tmpX, out var tmpY);
            y = tmpX - tmpY * (a / b);
            x = tmpY;
            
            return gcd;
        }

        private static readonly BigInteger FastSqrtSmallNumber = 4503599761588223UL;

        public static BigInteger SqrtFast(BigInteger value)
        {
            
            if (value <= FastSqrtSmallNumber)
            {
                if (value.Sign < 0) throw new ArgumentException("Negative argument.");
                return (UInt64)Math.Sqrt((UInt64)value);
            }

            BigInteger root;
            int byteLen = value.ToByteArray().Length;
            if (byteLen < 128)
            {
                root = (BigInteger)Math.Sqrt((Double)value);
            }
            else
            {
                root = (BigInteger)Math.Sqrt((Double)(value >> (byteLen - 127) * 8)) << (byteLen - 127) * 4;
            }

            for (; ; )
            {
                var root2 = value / root + root >> 1;
                if ((root2 == root || root2 == root + 1) && IsSqrt(value, root))
                {
                    return root;
                }
                root = value / root2 + root2 >> 1;
                if ((root == root2 || root == root2 + 1) && IsSqrt(value, root2))
                {
                    return root2;
                }
            }
        }

        private static Boolean IsSqrt(BigInteger value, BigInteger root)
        {
            var lowerBound = root * root;

            return value >= lowerBound && value <= lowerBound + (root << 1);
        }
    }
}
