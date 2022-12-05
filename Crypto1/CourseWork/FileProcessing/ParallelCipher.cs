using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.SymmetricAlgorithms.BlockCipherMode;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;
using CourseWork.SymmetricAlgorithms.Modes;
using CourseWork.SymmetricAlgorithms.Padding;

namespace CourseWork.FileProcessing
{
    public sealed class ParallelCipher : IParallelCipher
    {
        private Byte[] _iv;
        private Byte[] _copyIvForEncrypt;
        private Byte[] _copyIvForDecrypt;
        private ICipherAlgorithm _algorithm;
        private Padder _padder;
        private CipherTemplate _cipherTemplate;

        public ParallelCipher(ICipherAlgorithm algorithm, Byte[] iv)
        {
            _algorithm = algorithm;
            _iv = iv.ToArray();
            _copyIvForEncrypt = iv.ToArray();
            _copyIvForDecrypt = iv.ToArray();
            _padder = new Padder(PaddingType.PKCS7, _algorithm.GetBlockSize());
        }
        
        public async Task<Byte[]> Encrypt(string filePath, EncryptionMode encryptionMode)
        {
            _cipherTemplate = new CipherTemplateFactory().Create(_algorithm, encryptionMode);
            var processorCount = Environment.ProcessorCount;
            var blockReader = new BlockReader(filePath, _algorithm.GetBlockSize());
            var iterations = blockReader.GetBlocksNumber() % processorCount == 0
                ? blockReader.GetBlocksNumber() / processorCount
                : blockReader.GetBlocksNumber() / processorCount + 1;
            var outputBuffer = new Byte[iterations][];

            for (var count = 0; count < iterations; count++)
            {
                var blocks = await blockReader.GetNextBlocks(processorCount);
                if (count == iterations - 1)
                {
                    if (blocks[^1].Length == _algorithm.GetBlockSize())
                    {
                        blocks.Add(_padder.GetEmptyBlock());
                    }
                    else
                    {
                        blocks[^1] = _padder.PadBuffer(blocks.Last());
                    }
                }
                
                outputBuffer[count] = _cipherTemplate.Encrypt(blocks, ref _copyIvForEncrypt, count);
            }

            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public async Task<Byte[]> Decrypt(Byte[] inputBuffer, EncryptionMode encryptionMode)
        {
            _cipherTemplate = new CipherTemplateFactory().Create(_algorithm, encryptionMode);
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
                
                outputBuffer[count] = _cipherTemplate.Decrypt(currentBlock, ref _copyIvForDecrypt, count);
                
                if (count == outputBuffer.Count - 1)
                {
                    outputBuffer[^1] = _padder.RemovePadding(outputBuffer.Last());
                }
            }

            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public async Task Decrypt(String path, Byte[] inputBuffer, EncryptionMode encryptionMode)
        {
            _cipherTemplate = new CipherTemplateFactory().Create(_algorithm, encryptionMode);
            var blockWriter = new BlockWriter(path);
            var processorCount = Environment.ProcessorCount;
            var blocks = GetBlocksList(inputBuffer);
            var iterations = blocks.Count % processorCount == 0
                ? blocks.Count / processorCount
                : blocks.Count / processorCount + 1;

            for (var count = 0; count < iterations; count++)
            {
                var currentBlock = blocks
                    .Skip(count * processorCount)
                    .Take(processorCount)
                    .ToList();
                
                var decryptBlock = _cipherTemplate.Decrypt(currentBlock, ref _copyIvForDecrypt, count);
                
                if (count == iterations - 1)
                {
                    decryptBlock = _padder.RemovePadding(decryptBlock);
                }
                
                await blockWriter.WriteNextBlocks(decryptBlock);
            }
            blockWriter.Dispose();
        }
        
        private List<Byte[]> GetBlocksList(Byte[] inputBuffer)
        {
            Int32 blocksQuantity;
            
            if (inputBuffer.Length % _algorithm.GetBlockSize() == 0)
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
                var step = i * _algorithm.GetBlockSize();
                blocksArray.Add(new Byte[_algorithm.GetBlockSize()]);
                Array.Copy(sourceArray: inputBuffer,
                    sourceIndex: step,
                    destinationArray: blocksArray[i],
                    destinationIndex: 0,
                    length: _algorithm.GetBlockSize());
            }

            return blocksArray;
        }
    }
}