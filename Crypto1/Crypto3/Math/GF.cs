using System;
using System.Collections.Generic;

namespace Crypto3.Math
{
    public class GF
    {
        private byte _polynom;
        private readonly uint _basePolynome = 0b100011011; // For GF(256)



        public GF(byte Polynom)
        {
            this._polynom = Polynom;
        }

        public GF(byte Polynom, uint basePolynome)
        {
            this._polynom = Polynom;
            this._basePolynome = basePolynome;
        }

        public static bool CheckIrr(uint P)
        {
            for (uint i = 2; i < P; i++)
            {
                for (uint j = 2; j < P; j++)
                {

                    uint l = 0b10000000000;
                    uint r = 0b10000000000;

                    uint midResult = 0;
                    while (l != 0)
                    {
                        while (r != 0)
                        {
                            midResult ^= XMultiply((uint)(i & l), (uint)(j & r));

                            r >>= 1;
                        }
                        l >>= 1;
                        r = 0b10000000;
                    }

                    if (midResult == P) return false;
                }

            }
            return true;
        }

        public GF Inverse()
        {
            uint degree = 254;
            GF result = new GF(0b1);
            uint bitesForMask = degree;
            GF b = this;
            while (bitesForMask > 0)
            {
                if ((bitesForMask & 0b01) == 1)
                {
                    result = new GF(Module(result.Multiply(b)._polynom));
                }
                bitesForMask >>= 1;

                b = b.Multiply(b);
            }
            return result;
        }
        public GF Sum(GF right)
        {
            return new GF((byte)(_polynom ^ right._polynom));

        }
        public GF Multiply(GF right)
        {
            GF result = new GF(0);
            if (right._polynom == 0) return result;
            byte l = 0b10000000;
            byte r = 0b10000000;

            uint midResult = 0;
            while (l != 0)
            {
                while (r != 0)
                {
                    midResult ^= XMultiply((uint)(_polynom & l), (uint)(right._polynom & r));

                    r >>= 1;
                }
                l >>= 1;
                r = 0b10000000;
            }


            midResult = Module(midResult);

            return new GF((byte)midResult);
        }

        public GF Multiply(byte right)
        {
            return Multiply(new GF(right));
        }

        public byte GetPolynom()
        {
            return _polynom;
        }

        #region MathHelpers
        private static uint XMultiply(uint left, uint right) // x^4 * x^2 = x^6
        {
            uint result = left;
            if (right == 0) return 0;
            while (right != 1)
            {
                result <<= 1;
                right >>= 1;
            }
            return result;
        }

        private static uint GetHigherBitMask(uint n)
        {
            if (n == 0) return 0;
            uint count = 1;
            while (n != 1)
            {
                n >>= 1;
                count <<= 1;
            }

            return count;
        }

        private byte Module(uint num)
        {
            uint lDeg = GetHigherBitMask(num);
            uint baseDeg = GetHigherBitMask(_basePolynome);

            while (lDeg >= baseDeg)
            {
                uint tmpLeft = lDeg;
                uint tmpRight = baseDeg;
                while (tmpRight != 1)
                {
                    tmpLeft >>= 1;
                    tmpRight >>= 1;
                }

                uint tmpMulResult = XMultiply(_basePolynome, tmpLeft);
                num ^= tmpMulResult;
                lDeg = GetHigherBitMask(num);
            }

            return (byte)num;
        }

        public static List<UInt16> FindIrreduciblePolynomials()
        {
            List<UInt16> items = new List<UInt16>();
            var isSimple = false;
            for (var count = 257; count < 512; count += 2)
            {
                for (var secondCount = 3; secondCount < 32; secondCount++)
                {
                    if (ModeDivide((UInt16)count, (UInt16)secondCount) == 0)
                    {
                        isSimple = false;
                    }
                }
                if (isSimple)
                {
                    items.Add((UInt16)count);
                }

                isSimple = true;
            }

            return items;
        }
        private static uint ModeDivide(int _division, int _divisor)
        {
            uint division = (uint)_division;
            uint divisor = (uint)_divisor;

            if (division < divisor)
                throw new ArgumentException();

            if (divisor == 0)
                throw new DivideByZeroException();

            var maxNonNullBitForDivisor = GetHigherBitMask(divisor);

            var tmpDivision = division;
            var currMaxNonNullBitNumber = GetHigherBitMask(division);

            while (currMaxNonNullBitNumber >= maxNonNullBitForDivisor)
            {
                var tmpDivisor = (uint)((uint)divisor << (int)(currMaxNonNullBitNumber - maxNonNullBitForDivisor));
                tmpDivision ^= tmpDivisor;
                currMaxNonNullBitNumber = GetHigherBitMask(tmpDivision);
            }

            return tmpDivision;
        }
        #endregion



        public override string ToString()
        {
            return _polynom.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as GF);
        }

        public bool Equals(GF obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            if (this.GetType() != obj.GetType()) return false;
            return _polynom == obj._polynom;

        }
    }
}
