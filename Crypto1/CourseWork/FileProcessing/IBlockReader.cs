using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWork.FileProcessing
{
    public interface IBlockReader : IDisposable
    {
        public Task<List<Byte[]>> GetNextBlocks(Int32 numBlocks);

        public Int64 GetLength();

        public Int32 GetBlocksNumber();

        public Int32 GetBlocksCounted();
    }
}