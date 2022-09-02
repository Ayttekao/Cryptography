using System;

namespace CourseWork.SymmetricAlgorithms.LOKI97.Algorithm
{
    public static class PermutationGeneration
    {
        private const UInt32 PermutationSize = 0x100;
        private static readonly UInt64[] P = new UInt64[PermutationSize];

        private static void Init()
        {
            for (var i = 0; i < PermutationSize; i++)
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
            Init();
            return P;
        }
    }
}