using System;

namespace CourseWork.SymmetricAlgorithms.LOKI97.Algorithm.BlockPacker
{
    public interface IBlockPacker
    {
        public Tuple<UInt64, UInt64> PackBlock(Byte[] input, Int32 inOffset);
        public Byte[] UnpackBlock(UInt64 L, UInt64 R);
    }
}