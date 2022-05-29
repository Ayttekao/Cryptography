using System;
using System.Collections.Generic;

namespace CourseWork.FileProcessing
{
    public interface IBlockReader : IDisposable
    {
        public List<Byte[]> GetNextBlocks(Int32 numBlocks);

        public Int64 GetLength();

        public Int32 GetBlocksNumber();

        public Int32 GetBlocksCounted();
    }
}