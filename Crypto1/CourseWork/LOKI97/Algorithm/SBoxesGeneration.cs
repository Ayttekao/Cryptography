using System;

namespace CourseWork.LOKI97.Algorithm
{
    public static class SBoxesGeneration
    {
        /*S1-box options*/
        private const Int32 S1Gen = 0x2911;
        private const Int32 S1Size = 0x2000;
        private static readonly Byte[] S1 = new Byte[S1Size];

        /*S2-box options*/
        private const Int32 S2Gen = 0xAA7;
        private const Int32 S2Size = 0x800;
        private static readonly Byte[] S2 = new Byte[S2Size];

        private static void GenerationS1Box()
        {
            const Int32 s1Mask = S1Size - 1;

            for (var i = 0; i < S1Size; i++)
            { // for all S1 inputs
                var b = i ^ s1Mask; // compute input value
                S1[i] = Exp3(b, S1Gen, S1Size);
            }
        }

        private static void GenerationS2Box()
        {
            const Int32 s2Mask = S2Size - 1;

            for (var i = 0; i < S2Size; i++)
            { // for all S1 inputs
                var b = i ^ s2Mask; // compute input value
                S2[i] = Exp3(b, S2Gen, S2Size);
            }
        }

        private static Byte Exp3(Int32 b, Int32 g, Int32 n)
        {
            if (b == 0)
            {
                return 0;
            }
            var r = b;            // r = b ** 1
            b = Mult(r, b, g, n);  // r = b ** 2
            r = Mult(r, b, g, n);  // r = b ** 3
            return (Byte) r;
        }

        private static Int32 Mult(Int32 a, Int32 b, Int32 g, Int32 n)
        {
            var p = 0;
            while (b != 0)
            {
                if ((b & 0x01) != 0)
                    p ^= a;
                a <<= 1;
                if (a >= n)
                    a ^= g;
                b = (Int32)((UInt32)b >> 1);
            }
            return p;
        }

        public static Byte[] GetS1Box()
        {
            GenerationS1Box();
            return S1;
        }

        public static Byte[] GetS2Box()
        {
            GenerationS2Box();
            return S2;
        }

        public static void Init()
        {
            GenerationS1Box();
            GenerationS2Box();
        }
    }
}