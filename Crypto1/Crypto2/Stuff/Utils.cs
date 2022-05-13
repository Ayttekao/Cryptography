using System.Numerics;
using System.Security.Cryptography;

namespace Crypto2.Stuff
{
    internal static class Utils
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
    }
}
