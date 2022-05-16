namespace CourseWork.LOKI97.Algorithm
{
    public class SBoxesGeneration
    {
        /*S1-box options*/
        private static readonly int S1_GEN = 0x2911;
        private static readonly int S1_SIZE = 0x2000;
        private static readonly byte[] S1 = new byte[S1_SIZE];

        /*S2-box options*/
        private static readonly int S2_GEN = 0xAA7;
        private static readonly int S2_SIZE = 0x800;
        private static readonly byte[] S2 = new byte[S2_SIZE];

        private static void GenerationS1Box()
        {
            int S1_MASK = S1_SIZE - 1;

            for (int i = 0; i < S1_SIZE; i++)
            { // for all S1 inputs
                int b = i ^ S1_MASK; // compute input value
                S1[i] = Exp3(b, S1_GEN, S1_SIZE);
            }
        }

        private static void GenerationS2Box()
        {
            int S2_MASK = S2_SIZE - 1;

            for (int i = 0; i < S2_SIZE; i++)
            { // for all S1 inputs
                int b = i ^ S2_MASK; // compute input value
                S2[i] = Exp3(b, S2_GEN, S2_SIZE);
            }
        }

        private static byte Exp3(int b, int g, int n)
        {
            if (b == 0)
            {
                return 0;
            }
            var r = b;            // r = b ** 1
            b = Mult(r, b, g, n); // r = b ** 2
            r = Mult(r, b, g, n); // r = b ** 3
            return (byte) r;
        }

        private static int Mult(int a, int b, int g, int n)
        {
            var p = 0;
            while (b != 0)
            {
                if ((b & 0x01) != 0)
                    p ^= a;
                a <<= 1;
                if (a >= n)
                    a ^= g;
                b = (int)((uint)b >> 1);
            }
            return p;
        }

        public static byte[] GetS1Box()
        {
            return S1;
        }

        public static byte[] GetS2Box()
        {
            return S2;
        }

        public static void Init()
        {
            GenerationS1Box();
            GenerationS2Box();
        }
    }
}