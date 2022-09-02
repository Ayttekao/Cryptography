using System;

namespace CourseWork.SymmetricAlgorithms.LOKI97.Algorithm.EncryptionTransformation
{
    public class Encryption : IEncryptionTransformation
    {
        private Byte[]  S1 = SBoxesGeneration.GetS1Box();
        private Byte[]  S2 = SBoxesGeneration.GetS2Box();
        private UInt64[] P = PermutationGeneration.GetPermutation();
        
        public UInt64 Compute(UInt64 A, UInt64 B)
        {
            var Al = (int) (A >> 32);
            var Ar = (int) A;
            var Br = (int) B;
            var d = ((UInt64) ((Al & ~Br) | (Ar & Br)) << 32) |
                    ((UInt64) ((Ar & ~Br) | (Al & Br)) & 0xFFFFFFFFL);
    
            UInt64 e = P[S1[(Int32) ((d >> 56 | d << 8) & 0x1FFF)] & 0xFF] >> 7 |
                       P[S2[(Int32) ((d >> 48) & 0x7FF)] & 0xFF] >> 6 |
                       P[S1[(Int32) ((d >> 40) & 0x1FFF)] & 0xFF] >> 5 |
                       P[S2[(Int32) ((d >> 32) & 0x7FF)] & 0xFF] >> 4 |
                       P[S2[(Int32) ((d >> 24) & 0x7FF)] & 0xFF] >> 3 |
                       P[S1[(Int32) ((d >> 16) & 0x1FFF)] & 0xFF] >> 2 |
                       P[S2[(Int32) ((d >> 8) & 0x7FF)] & 0xFF] >> 1 |
                       P[S1[(Int32) (d & 0x1FFF)] & 0xFF];
    
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
}