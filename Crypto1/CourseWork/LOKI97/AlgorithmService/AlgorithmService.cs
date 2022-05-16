using System;
using System.Collections.Generic;
using CourseWork.LOKI97.Algorithm;
using CourseWork.LOKI97.AlgorithmService.Modes;
using CourseWork.LOKI97.AlgorithmService.Padding;

namespace CourseWork.LOKI97.AlgorithmService
{
    public class AlgorithmService
    {
        static readonly int blockSize = 16;

        public static byte[] RunAlgorithm(byte[] inputBuffer, byte[] keyBuffer, byte[] initializationVector, EncryptionMode encryptionMode, bool doEncrypt)
        {
            List<byte[]> blocksList;
            var keyGeneration = new KeyGeneration();
            var padder = new Padder(PaddingType.PKCS7, blockSize);
            var key = keyGeneration.MakeKey(keyBuffer);
            var mode = ModeFactory.CreateEncryptionMode(encryptionMode);

            if (doEncrypt)
            {
                inputBuffer = padder.PadBuffer(inputBuffer);
                blocksList = GetBlocksList(inputBuffer);
                return mode.Encrypt(blocksList, key, initializationVector);
            } 
            else
            {
                blocksList = GetBlocksList(inputBuffer);
                var badCharCount = 0;
                var outputBuffer = padder.RemovePadding(mode.Decrypt(blocksList, key, initializationVector));
                    
                for (var index = 0; index < 15; index++)
                {
                    if (outputBuffer[outputBuffer.Length - index - 1] == 0)
                    {
                        badCharCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                
                return CopyOfRange(outputBuffer, 0, outputBuffer.Length - badCharCount);
            }
        }

        private static List<Byte[]> GetBlocksList(Byte[] inputBuffer)
        {
            Int32 blocksQuantity;
            
            if (inputBuffer.Length % blockSize == 0)
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
                var step = i * blockSize;
                blocksArray.Add(CopyOfRange(inputBuffer, step, blockSize + step));
            }

            return blocksArray;
        }

        private static byte[] CopyOfRange (Byte[] src, int start, int end) {
            var len = end - start;
            var dest = new byte[len];
            
            for (var index = 0; index < len; index++)
            {
                dest[index] = src[start + index];
            }
            return dest;
        }
    }
}