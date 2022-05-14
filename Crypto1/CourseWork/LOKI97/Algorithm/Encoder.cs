using System;
using CourseWork.LOKI97.Algorithm;

namespace CourseWork.LOKI97
{
    public class Encoder
    {
        static readonly int ROUNDS = 16;

        public byte[] BlockEncrypt(byte[] input, int inOffset, Object sessionKey)
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
            UInt64 nR, f_out;
            UInt32 k = 0;
            for (int i = 0; i < ROUNDS; i++)
            {
                nR = R + SK[k++];
                f_out = Cipher.Compute(nR, SK[k++]);
                nR += SK[k++];
                R = L ^ f_out;
                L = nR;

            }

            // unpack resulting L & R into out buffer
            byte[] result = {
                (byte) (R >> 56), (byte) (R >> 48),
                (byte) (R >> 40), (byte) (R >> 32),
                (byte) (R >> 24), (byte) (R >> 16),
                (byte) (R >> 8), (byte) R,
                (byte) (L >> 56), (byte) (L >> 48),
                (byte) (L >> 40), (byte) (L >> 32),
                (byte) (L >> 24), (byte) (L >> 16),
                (byte) (L >> 8), (byte) L
            };


            return result;
        }
    }
}