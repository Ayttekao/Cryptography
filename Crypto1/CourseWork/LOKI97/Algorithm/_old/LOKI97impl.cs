/*using System;

namespace CourseWork.LOKI97.Algorithm._old
{
    public class LOKI97impl
    {
        private const UInt32 Rounds = 16;
        private const UInt32 NumSubKeys = 48;
        private Byte[] S1 = SBoxesGeneration.GetS1Box();
        private Byte[] S2 = SBoxesGeneration.GetS2Box();
        private UInt64[] P = PermutationGeneration.GetPermutation();
        
        public byte[] BlockEncrypt(byte[] input, int inOffset, object sessionKey)
        {
            UInt64[] SK = (UInt64[]) sessionKey;    // local ref to session key

            // pack input block into 2 longs: L and R
            var (L, R) = PackBlock(input, inOffset);

            // compute all rounds for this 1 block
            UInt32 k = 0;
            for (var i = 0; i < Rounds; i++)
            {
                var nR = R + SK[k++];
                var fOut = Compute(nR, SK[k++]);
                nR += SK[k++];
                R = L ^ fOut;
                L = nR;

            }

            // unpack resulting L & R into out buffer
            return UnpackBlock(L, R);
        }

        public byte[] BlockDecrypt(byte[] input, int inOffset, object sessionKey)
        {
            UInt64[] SK = (UInt64[]) sessionKey;    // local ref to session key

            // pack input block into 2 longs: L and R
            var (L, R) = PackBlock(input, inOffset);

            // compute all rounds for this 1 block
            UInt32 k = NumSubKeys - 1;
            for (var i = 0; i < Rounds; i++)
            {
                var nR = R - SK[k--];
                var fOut = Compute(nR, SK[k--]);
                nR -= SK[k--];
                R = L ^ fOut;
                L = nR;
            }

            // unpack resulting L & R into out buffer
            return UnpackBlock(L, R);
        }

        private Tuple<UInt64, UInt64> PackBlock(Byte[] input, Int32 inOffset)
        {
            UInt64 L = (input[inOffset++] & 0xFFUL) << 56 |
                       (input[inOffset++] & 0xFFUL) << 48 |
                       (input[inOffset++] & 0xFFUL) << 40 |
                       (input[inOffset++] & 0xFFUL) << 32 |
                       (input[inOffset++] & 0xFFUL) << 24 |
                       (input[inOffset++] & 0xFFUL) << 16 |
                       (input[inOffset++] & 0xFFUL) << 8 |
                       (input[inOffset++] & 0xFFUL);
            UInt64 R = (input[inOffset++] & 0xFFUL) << 56 |
                       (input[inOffset++] & 0xFFUL) << 48 |
                       (input[inOffset++] & 0xFFUL) << 40 |
                       (input[inOffset++] & 0xFFUL) << 32 |
                       (input[inOffset++] & 0xFFUL) << 24 |
                       (input[inOffset++] & 0xFFUL) << 16 |
                       (input[inOffset++] & 0xFFUL) << 8 |
                       (input[inOffset] & 0xFFUL);

            return new Tuple<UInt64, UInt64>(L, R);
        }

        private Byte[] UnpackBlock(UInt64 L, UInt64 R)
        {
            return new Byte[] {
                (Byte) (R >> 56), (Byte) (R >> 48),
                (Byte) (R >> 40), (Byte) (R >> 32),
                (Byte) (R >> 24), (Byte) (R >> 16),
                (Byte) (R >> 8), (Byte) R,
                (Byte) (L >> 56), (Byte) (L >> 48),
                (Byte) (L >> 40), (Byte) (L >> 32),
                (Byte) (L >> 24), (Byte) (L >> 16),
                (Byte) (L >> 8), (Byte) L
            };
        }
        
        public UInt64 Compute(UInt64 A, UInt64 B)
        {
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