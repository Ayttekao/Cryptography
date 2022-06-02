using System;
using System.Threading.Tasks;

namespace CourseWork.FileProcessing;

public interface IBlockWriter : IDisposable
{
    public Task WriteNextBlocks(Byte[] blocks);
    public Int32 GetBlocksRecorded();
}