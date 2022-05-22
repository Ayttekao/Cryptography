using System;

namespace CourseWork.LOKI97.Algorithm
{
    public class Decoder
    {
        private static readonly UInt32 ROUNDS = 16;
        private static readonly UInt32 NUM_SUBKEYS = 48;

        public Byte[] BlockDecrypt(Byte[] input, int inOffset, Object sessionKey)
        {

            UInt64[] SK = (UInt64[]) sessionKey;    // local ref to session key

            // pack input block into 2 longs: L and R
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
                       (input[inOffset++] & 0xFFUL);

            // compute all rounds for this 1 block
            UInt64 nR;
            UInt64 fOut;
            UInt32 k = NUM_SUBKEYS - 1;
            for (var i = 0; i < ROUNDS; i++)
            {
                nR = R - SK[k--];
                fOut = Cipher.Compute(nR, SK[k--]);
                nR -= SK[k--];
                R = L ^ fOut;
                L = nR;
            }

            // unpack resulting L & R into out buffer
            Byte[] result = {
                (Byte) (R >> 56), (Byte) (R >> 48),
                (Byte) (R >> 40), (Byte) (R >> 32),
                (Byte) (R >> 24), (Byte) (R >> 16),
                (Byte) (R >> 8), (Byte) R,
                (Byte) (L >> 56), (Byte) (L >> 48),
                (Byte) (L >> 40), (Byte) (L >> 32),
                (Byte) (L >> 24), (Byte) (L >> 16),
                (Byte) (L >> 8), (Byte) L
            };

            return result;
        }
    }
}