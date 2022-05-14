using System;
using System.Numerics;

namespace Crypto2.Stuff
{
    public static class Functions
    {
        /*
         * 1. Спроектируйте и реализуйте сервис, позволяющий вычислять значения символов Лежандра и Якоби.
         */
        public static BigInteger Jacobi(BigInteger a, BigInteger n)
        {
            if (a == 1) return 1;

            var value = (n - 1) / 2 % 2 == 0 ? 1 : -1;
            if (a < 0) return Jacobi(-a, n) * value;

            value = (n * n - 1) / 8 % 2 == 0 ? 1 : -1;
            if (a % 2 == 0) return Jacobi(a / 2, n) * value;

            value = (a - 1) * (n - 1) / 4 % 2 == 0 ? 1 : -1;
            return value * Jacobi(n % a, a);
        }
        public static BigInteger Legendre(BigInteger a, BigInteger p)
        {
            if (p < 2)
            {
                throw new ArgumentException(nameof(p));
            }
            
            a %= p;
            if (a == 0) return 0;

            if (a == 1) return 1;

            int value = (a - 1) * (p - 1) / 4 % 2 == 0 ? 1 : -1;
            if (a % 2 != 0) return Legendre(p % a, a) * value;

            value = (p * p - 1) / 8 % 2 == 0 ? 2 : 1;
            return Legendre(a / 2, p) * value;
        }
    }
}