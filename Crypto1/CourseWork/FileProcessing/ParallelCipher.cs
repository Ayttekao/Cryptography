using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;
using CourseWork.LOKI97.AlgorithmService.Modes;
using CourseWork.LOKI97.AlgorithmService.Padding;

namespace CourseWork.FileProcessing
{
    public class ParallelCipher : IParallelCipher
    {
        private Byte[] _iv;
        private ICipherAlgorithm _algorithm;
        private Int32 _blockSize;
        private Padder _padder;

        public ParallelCipher(ICipherAlgorithm algorithm, Byte[] iv, Int32 blockSize)
        {
            _algorithm = algorithm;
            _iv = iv.ToArray();
            _blockSize = blockSize;
            _padder = new Padder(PaddingType.PKCS7, blockSize);
        }
        
        public byte[] Encrypt(string filePath, EncryptionMode encryptionMode)
        {
            var symmetricCipherAlgo = ModeFactory.CreateEncryptionMode(encryptionMode);
            var processorCount = Environment.ProcessorCount;
            var blockReader = new BlockReader(filePath, _blockSize);
            Int32 iterations = blockReader.GetBlocksNumber() / processorCount;
            var outputBuffer = new Byte[blockReader.GetBlocksNumber()][];

            for (var block = 0; block < iterations; block++)
            {
                var blocks = blockReader.GetNextBlocks(processorCount);
                var blockLocal = block;
                Parallel.For(0, blocks.Count, index =>
                {
                    if (blockLocal == iterations - 1 && index == blocks.Count - 1 && blocks[index].Length != _blockSize)
                    {
                        blocks[index] = _padder.PadBuffer(blocks[index]);
                    }

                    outputBuffer[index + blockLocal * processorCount] = _algorithm.BlockEncrypt(blocks[index], 0);
                });
                
                /*for (var index = 0; index < blocks.Count; index++)
                {
                    if (block == iterations - 1 && index == blocks.Count - 1 && blocks[index].Length != _blockSize)
                    {
                        blocks[index] = _padder.PadBuffer(blocks[index]);
                    }
                    outputBuffer[index + block * processorCount] = _algorithm.BlockEncrypt(blocks[index], 0);
                }*/
            }

            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public Byte[] Decrypt(Byte[] inputBuffer, EncryptionMode encryptionMode)
        {
            var outputBuffer = GetBlocksList(inputBuffer);
            var symmetricCipherAlgo = ModeFactory.CreateEncryptionMode(encryptionMode);
            var processorCount = Environment.ProcessorCount;

            Parallel.For(0, outputBuffer.Count, index =>
            {
                outputBuffer[index] = _algorithm.BlockDecrypt(outputBuffer[index], 0);
                if (index == outputBuffer.Count - 1)
                {
                    outputBuffer[index] = _padder.RemovePadding(outputBuffer[index]);
                }
            });
            
            /*for (var index = 0; index < outputBuffer.Count; index++)
            {
                outputBuffer[index] = _algorithm.BlockDecrypt(outputBuffer[index], 0);
                if (index == outputBuffer.Count - 1)
                {
                    outputBuffer[index] = _padder.RemovePadding(outputBuffer[index]);
                }
            }*/

            return outputBuffer.SelectMany(x => x).ToArray();
        }
        
        private List<Byte[]> GetBlocksList(Byte[] inputBuffer)
        {
            Int32 blocksQuantity;
            
            if (inputBuffer.Length % _blockSize == 0)
            {
                blocksQuantity = inputBuffer.Length / 16;
            }
            else
            {
                blocksQuantity = inputBuffer.Length / 16 + 1;
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