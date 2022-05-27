/*using System;

namespace CourseWork.LOKI97.Algorithm
{
    public static class Cipher
    {
        public static UInt64 Compute(UInt64 A, UInt64 B)
        {
            var S1 = SBoxesGeneration.GetS1Box();
            var S2 = SBoxesGeneration.GetS2Box();
            var P = PermutationGeneration.GetPermutation();
    
            var Al = (int) (A >> 32);
            var Ar = (int) A;
            var Br = (int) B;
            var d = ((UInt64) ((Al & ~Br) | (Ar & Br)) << 32) |
                    ((UInt64) ((Ar & ~Br) | (Al & Br)) & 0xFFFFFFFFL);
    
            // Compute e = P(Sa(d))
            //    mask out each group of 12 bits for E
            //    then compute first S-box column [S1,S2,S1,S2,S2,S1,S2,S1]
            //    permuting output through P (with extra shift to build full P)
    
            UInt64 e = P[S1[(Int32) ((d >> 56 | d << 8) & 0x1FFF)] & 0xFF] >> 7 |
                       P[S2[(Int32) ((d >> 48) & 0x7FF)] & 0xFF] >> 6 |
                       P[S1[(Int32) ((d >> 40) & 0x1FFF)] & 0xFF] >> 5 |
                       P[S2[(Int32) ((d >> 32) & 0x7FF)] & 0xFF] >> 4 |
                       P[S2[(Int32) ((d >> 24) & 0x7FF)] & 0xFF] >> 3 |
                       P[S1[(Int32) ((d >> 16) & 0x1FFF)] & 0xFF] >> 2 |
                       P[S2[(Int32) ((d >> 8) & 0x7FF)] & 0xFF] >> 1 |
                       P[S1[(Int32) (d & 0x1FFF)] & 0xFF];
    
            // Compute f = Sb(e,B)
            //    where the second S-box column is [S2,S2,S1,S1,S2,S2,S1,S1]
            //    for each S, lower bits come from e, upper from upper half of B
    
            UInt64 f =
                    (UInt64)((S2[(UInt32) (((e >> 56) & 0xFF) | ((B >> 53) & 0x700))] & 0xFFL) << 56 |
                            (S2[(UInt32) (((e >> 48) & 0xFF) | ((B >> 50) & 0x700))] & 0xFFL) << 48 |
                            (S1[(UInt32) (((e >> 40) & 0xFF) | ((B >> 45) & 0x1F00))] & 0xFFL) << 40 |
                            (S1[(UInt32) (((e >> 32) & 0xFF) | ((B >> 40) & 0x1F00))] & 0xFFL) << 32 |
                            (S2[(UInt32) (((e >> 24) & 0xFF) | ((B >> 37) & 0x700))] & 0xFFL) << 24 |
                            (S2[(UInt32) (((e >> 16) & 0xFF) | ((B >> 34) & 0x700))] & 0xFFL) << 16 |
                            (S1[(UInt32) (((e >> 8) & 0xFF) | ((B >> 29) & 0x1F00))] & 0xFFL) << 8 |
                            (S1[(UInt32) ((e & 0xFF) | ((B >> 24) & 0x1F00))] & 0xFFL));
    
            return f;
        }
    }
}*/