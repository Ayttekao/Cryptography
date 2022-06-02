using System;
using System.IO;
using System.Threading.Tasks;

namespace CourseWork.FileProcessing;

public class BlockWriter : IBlockWriter
{
    private FileStream _fileStream;
    private Int32 _blocksRecorded;

    public BlockWriter(String path)
    {
        _blocksRecorded = 0;
        _fileStream = File.OpenWrite(path);
    }

    ~BlockWriter()
    {
        _fileStream?.Dispose();
    }

    public void Dispose()
    {
        _fileStream?.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task WriteNextBlocks(Byte[] blocks)
    {
        await _fileStream.WriteAsync(blocks.AsMemory(0, blocks.Length));
        _blocksRecorded++;
    }

    public Int32 GetBlocksRecorded()
    {
        return _blocksRecorded;
    }
}