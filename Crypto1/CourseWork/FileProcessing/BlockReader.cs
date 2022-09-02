using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CourseWork.FileProcessing
{
    public class BlockReader : IBlockReader
    {
        private FileStream _fileStream;
        private Int32 _blockSize;
        private Int32 _blocksCounted;
        private Int32 _blocksNumber;
        
        public BlockReader(String path, Int32 blockSize)
        {
            _blocksCounted = 0;
            _fileStream = File.OpenRead(path);
            _blockSize = blockSize;
            _blocksNumber = (int)(_fileStream.Length % blockSize == 0
                ? _fileStream.Length / _blockSize
                : _fileStream.Length / _blockSize + 1);
        }

        ~BlockReader()
        {
            _fileStream?.Dispose();
        }

        public Task<List<Byte[]>> GetNextBlocks(Int32 numBlocks)
        {
            var blocksList = new List<Byte[]>();
            var bufferSize = _fileStream.Length - _fileStream.Position < _blockSize * numBlocks
                ? _fileStream.Length - _fileStream.Position
                : _blockSize * numBlocks;

            if (bufferSize != 0)
            {
                var buffer = new Byte[bufferSize];
                _fileStream.Read(buffer, offset: 0, count: buffer.Length);

                for (var index = 0; index * _blockSize < bufferSize; index++)
                {
                    var length = (index + 1) * _blockSize < bufferSize
                        ? _blockSize 
                        : bufferSize - index * _blockSize;
                    
                    blocksList.Add(new Byte[length]);
                    Array.Copy(sourceArray: buffer,
                        sourceIndex: _blockSize * index,
                        destinationArray: blocksList[index],
                        destinationIndex: 0,
                        length: blocksList[index].Length);
                    _blocksCounted++;
                }
            }

            return Task.FromResult(blocksList); 
        }

        public Int64 GetLength()
        {
            return _fileStream.Length;
        }

        public Int32 GetBlocksNumber()
        {
            return _blocksNumber;
        }

        public Int32 GetBlocksCounted()
        {
            return _blocksCounted;
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}