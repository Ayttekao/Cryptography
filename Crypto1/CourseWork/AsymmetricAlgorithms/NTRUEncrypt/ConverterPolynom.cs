using System;

namespace CourseWork.AsymmetricAlgorithms.NTRUEncrypt
{
    public static class ConverterPolynom
    {
        public static int[] ByteToCoef(byte[] arrayByte)
        {
            var array = new int[ConstantsNtru.N];

            for (var i = 0; i < arrayByte.Length * 8; i++)
            {
                array[i] = (arrayByte[i / 8] >> (i % 8)) & 1;
            }
            array[arrayByte.Length * 8] = 1;

            return array;
        }

        public static byte[] CoefToByte(int[] arrayInt)
        {
            var index = arrayInt.Length - 1;
            while (arrayInt[index] != 1)
            {
                index--;
            }

            if (index % 8 != 0)
            {
                throw new ArgumentException("Incorrect array");
            }
            var arrayByte = new byte[index / 8];

            for (var i = 0; i < index; i++)
            {
                arrayByte[i / 8] = (byte)(arrayByte[i / 8] | (arrayInt[i] << (i % 8)));
            }

            return arrayByte;
        }

        public static byte[] PolynomToByte(PolynomModQn polynom)
        {
            var kolBit = Convert.ToString(ConstantsNtru.q - 1, 2).Length;
            var res = new byte[polynom.Сoefficient.Length * kolBit / 8 + 1];

            for (var i = 0; i < polynom.Сoefficient.Length * kolBit; i++)
            {
                var x = (byte)(((polynom.Сoefficient[i / kolBit] >> (i % kolBit)) & 1) << (i % 8));
                res[i / 8] = (byte)(res[i / 8] | x);
            }
            return res;
        }

        public static PolynomModQn ByteToPolynom(byte[] arrayByte)
        {
            var kolBit = Convert.ToString(ConstantsNtru.q - 1, 2).Length;
            var arrayInt = new int[arrayByte.Length * 8 / kolBit];

            for (var i = 0; i < arrayInt.Length * kolBit; i++)
            {
                var x = ((arrayByte[i / 8] >> (i % 8)) & 1) << (i % kolBit);
                arrayInt[i / kolBit] |= x;
            }
            return new PolynomModQn(arrayInt, ConstantsNtru.q, ConstantsNtru.N);
        }
    }
}
