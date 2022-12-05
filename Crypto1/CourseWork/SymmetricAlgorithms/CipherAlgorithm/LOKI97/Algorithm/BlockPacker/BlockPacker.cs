using System;

namespace CourseWork.SymmetricAlgorithms.CipherAlgorithm.LOKI97.Algorithm.BlockPacker
{
    public class BlockPacker : IBlockPacker
    {
        public Tuple<UInt64, UInt64> PackBlock(Byte[] input, Int32 inOffset)
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

        public Byte[] UnpackBlock(UInt64 L, UInt64 R)
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
    }
}