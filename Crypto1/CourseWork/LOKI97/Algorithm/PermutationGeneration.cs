using System;

namespace CourseWork.LOKI97.Algorithm
{
    public class PermutationGeneration
    {
        private static readonly UInt32 PERMUTATION_SIZE = 0x100;
        private static readonly UInt64[] P = new UInt64[PERMUTATION_SIZE];

        public static void Init()
        {
            for (var i = 0; i < PERMUTATION_SIZE; i++)
            {
                var pval = (UInt64)0;

                for (int j = 0, k = 7; j < 8; j++, k += 8)
                {
                    pval |= (((UInt64)i >> j) & 0x1) << k;
                }
                P[i] = pval;
            }
        }

        public static UInt64[] GetPermutation()
        {
            return P;
        }
    }
}