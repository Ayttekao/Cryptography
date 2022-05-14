using System;
using System.Collections.Generic;

namespace Crypto3.Stuff
{
    /* 
     * 1. Реализуйте stateless-сервис, предоставляющий объектный функционал для:
     *     - сложения двоичных полиномов (далее - элементов) из 𝐺𝐹(2^8)
     *     - умножения элементов из по заданному модулю 𝐺𝐹(2^8)
     *     - взятия обратного элемента для элемента из по 𝐺𝐹(2^8) заданному модулю;
     *     - проверки двоичного полинома степени 8 на неприводимость над 𝐺𝐹(2^8)
     *     - генерации коллекции всех неприводимых над 𝐺𝐹(2^8) двоичных полиномов степени 8
     * При попытке выполнения операции по приводимому над полем модулю, генерируйте (и перехватывайте в вызывающем коде)
     * исключительную ситуацию. Значения элементов из и 𝐺𝐹(2^8) модулей над передавайте и возвращайте в виде
     * однобайтовых 𝐺𝐹(2^8) значений (byte, char, ...). При вычислениях максимизируйте использование битовых операций.
     */
    public static class GaloisField
    {
        public static Byte Add(Byte lhs, Byte rhs)
        {
            return (Byte)(lhs ^ rhs);
        }

        public static Byte Multiply(Byte lhs, Byte rhs)
        {
            Byte resultPoly = 0;
            Byte counter;
            for (counter = 0; counter < 8; counter++) 
            {
                if ((rhs & 1) != 0)
                {
                    resultPoly ^= lhs;
                }

                var hiBitSet = (Byte)(lhs & 0x80);
                lhs <<= 1;
                if (hiBitSet != 0)
                {
                    lhs ^= 0x1b;
                }

                rhs >>= 1;
            }

            return resultPoly;
        }

        public static UInt64 MultiplyMod(UInt32 lhs, UInt32 rhs, UInt16 modulo)
        {
            if (lhs > rhs) 
            {
                lhs = lhs ^ rhs;
                rhs = lhs ^ rhs;
                lhs = lhs ^ rhs;
            }

            UInt64 result = 0;
            var tmpRhs = (UInt64)rhs;
            while (lhs > 0)
            {
                if ((lhs & 1) != 0)
                {
                    result = (result + tmpRhs) % modulo;
                }

                tmpRhs = (tmpRhs << 1) % modulo;
                lhs >>= 1;
            }

            return result;
        }

        public static UInt32 Inverse(Byte value, UInt16 modulo)
        {
            UInt32 result = 1;
            UInt16 pow = 254;
            UInt32 tmp = value;
            while (pow > 0) 
            {
                if ((pow & 1) != 0)
                {
                    result = (UInt32)MultiplyMod(result, tmp, modulo);
                }

                tmp = (UInt32)MultiplyMod(tmp, tmp, modulo);
                pow >>= 1;
            }

            return result;
        }

        public static UInt16 ExtentFunc(UInt16 value)
        {
            UInt16 number = 0;
            UInt16 one = 1;
            
            for (Byte counter = 0 ; counter < 13; counter++) 
            {
                if ((one & value) != 0)
                {
                    number = counter;
                }

                one <<= 1;
            }

            return number;
        }

        public static UInt16 DivideMod(UInt16 lhs, UInt16 rhs)
        {
            var firstExtent = ExtentFunc(rhs);
            var secondExtent = ExtentFunc(lhs);

            while (firstExtent <= secondExtent)
            {
                var tmp = (UInt16)(rhs << (secondExtent - firstExtent));
                lhs ^= tmp;
                secondExtent = ExtentFunc(lhs);
            }

            return lhs;
        }

        public static List<UInt16> FindIrreduciblePolynomials()
        {
            List<UInt16> items = new List<UInt16>();
            var isSimple = false;
            for (var count = 257; count < 512; count += 2)
            {
                for (var secondCount = 3; secondCount < 32; secondCount++) 
                {
                    if (DivideMod((UInt16)count, (UInt16)secondCount) == 0)
                    {
                        isSimple = false;
                    }
                }
                if (isSimple) {
                    items.Add((UInt16)count);
                }

                isSimple = true;
            }

            return items;
        }
    }
}