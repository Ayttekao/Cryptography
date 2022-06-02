using System;
using System.Collections.Generic;
using System.Numerics;

namespace CourseWork.Benaloh.Algorithm
{
    class PollardsRhoSolver
    {
        public BigInteger GreatestCommonDivisorRec(BigInteger x1, BigInteger x2)
        {
            return x2 == 0 ? x1 : GreatestCommonDivisorRec(x2, x1%x2);
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
        
        public bool IsPrime(BigInteger n)
        {
            for(int i = 2; i <= SqrtFast(n); i++){
                if(n % i == 0){
                    return false;
                }
            }

            return true;
        }

        public void GetFactors(List<BigInteger> acc, BigInteger n)
        {
            if(n == 1){
                return;
            }

            if(IsPrime(n))
            {
                acc.Add(n);
                return;
            }
            
            BigInteger divisor = Rho(n);
            GetFactors(acc, divisor);
            GetFactors(acc, n/divisor);
        }

        private BigInteger Rho(BigInteger num) 
        {
            BigInteger x1 = 2, x2 = 2, divisor;        

            if (num % 2 == 0) 
                return 2;
                
            do
            {
                x1 = Func(x1) % num;
                x2 = Func(Func(x2)) % num;
                divisor = GreatestCommonDivisorRec(BigInteger.Abs(x1 - x2), num);
            } while (divisor == 1);
            return divisor;
        }

        private BigInteger Func(BigInteger x)    
        {
            //X^2 + C
            return x * x + 1;
        } 
    }
}