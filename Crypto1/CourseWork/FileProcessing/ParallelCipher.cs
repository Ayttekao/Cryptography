using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;
using CourseWork.LOKI97.AlgorithmService.Modes;
using CourseWork.LOKI97.AlgorithmService.Padding;
using CourseWork.Template;

namespace CourseWork.FileProcessing
{
    public class ParallelCipher : IParallelCipher
    {
        private Byte[] _iv;
        private Byte[] _copyIvForEncrypt;
        private Byte[] _copyIvForDecrypt;
        private ICipherAlgorithm _algorithm;
        private Int32 _blockSize;
        private Padder _padder;
        private CipherTemplate _cipherTemplate;

        public ParallelCipher(ICipherAlgorithm algorithm, Byte[] iv, Int32 blockSize)
        {
            _algorithm = algorithm;
            _iv = iv.ToArray();
            _copyIvForEncrypt = iv.ToArray();
            _copyIvForDecrypt = iv.ToArray();
            _blockSize = blockSize;
            _padder = new Padder(PaddingType.PKCS7, blockSize);
        }
        
        public async Task<Byte[]> Encrypt(string filePath, EncryptionMode encryptionMode)
        {
            _cipherTemplate = new CipherTemplateFactory().CreateCipherTemplate(_algorithm, encryptionMode);
            var processorCount = Environment.ProcessorCount;
            var blockReader = new BlockReader(filePath, _blockSize);
            var iterations = blockReader.GetBlocksNumber() % processorCount == 0
                ? blockReader.GetBlocksNumber() / processorCount
                : blockReader.GetBlocksNumber() / processorCount + 1;
            var outputBuffer = new Byte[iterations][];

            for (var count = 0; count < iterations; count++)
            {
                var blocks = await blockReader.GetNextBlocks(processorCount);
                if (count == iterations - 1)
                {
                    if (blocks[^1].Length == _blockSize)
                    {
                        blocks.Add(Enumerable.Repeat((Byte)_blockSize, _blockSize).ToArray());
                    }
                    else
                    {
                        blocks[^1] = _padder.PadBuffer(blocks.Last());
                    }
                }
                
                outputBuffer[count] = _cipherTemplate.Run(blocks, ref _copyIvForEncrypt, count, true);
            }

            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public async Task<Byte[]> Decrypt(Byte[] inputBuffer, EncryptionMode encryptionMode)
        {
            _cipherTemplate = new CipherTemplateFactory().CreateCipherTemplate(_algorithm, encryptionMode);
            var processorCount = Environment.ProcessorCount;
            var blocks = GetBlocksList(inputBuffer);
            var iterations = blocks.Count % processorCount == 0
                ? blocks.Count / processorCount
                : blocks.Count / processorCount + 1;
            var outputBuffer = Enumerable.Repeat(default(Byte[]), iterations).ToList();

            for (var count = 0; count < iterations; count++)
            {
                var currentBlock = blocks
                    .Skip(count * processorCount)
                    .Take(processorCount)
                    .ToList();
                
                outputBuffer[count] = _cipherTemplate.Run(currentBlock, ref _copyIvForDecrypt, count, false);
                
                if (count == outputBuffer.Count - 1)
                {
                    outputBuffer[^1] = _padder.RemovePadding(outputBuffer.Last());
                }
            }

            return outputBuffer.SelectMany(x => x).ToArray();
        }
        
        private List<Byte[]> GetBlocksList(Byte[] inputBuffer)
        {
            Int32 blocksQuantity;
            
            if (inputBuffer.Length % _blockSize == 0)
            {
                blocksQuantity = inputBuffer.Length / _algorithm.GetBlockSize();
            }
            else
            {
                blocksQuantity = inputBuffer.Length / _algorithm.GetBlockSize() + 1;
            }
            
            var blocksArray = new List<Byte[]>(blocksQuantity);

            for (var i = 0; i < blocksQuantity; i++)
            {
                var step = i * _blockSize;
                blocksArray.Add(CopyOfRange(inputBuffer, step, _blockSize + step));
            }

            return blocksArray;
        }

        private static Byte[] CopyOfRange (Byte[] src, int start, int end) {
            var len = end - start;
            var dest = new Byte[len];
            
            for (var index = 0; index < len; index++)
            {
                dest[index] = src[start + index];
            }
            return dest;
        }
    }
}