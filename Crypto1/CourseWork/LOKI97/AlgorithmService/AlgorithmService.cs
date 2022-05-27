using System;
using System.Collections.Generic;
using CourseWork.LOKI97.Algorithm;
using CourseWork.LOKI97.Algorithm.BlockPacker;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;
using CourseWork.LOKI97.Algorithm.EncryptionTransformation;
using CourseWork.LOKI97.Algorithm.KeyGen;
using CourseWork.LOKI97.AlgorithmService.Modes;
using CourseWork.LOKI97.AlgorithmService.Padding;

namespace CourseWork.LOKI97.AlgorithmService
{
    public class AlgorithmService
    {
        static readonly int blockSize = 16;

        public static Byte[] RunAlgorithm(Byte[] inputBuffer, Byte[] keyBuffer, Byte[] initializationVector, EncryptionMode encryptionMode, Boolean doEncrypt)
        {
            List<Byte[]> blocksList;
            var padder = new Padder(PaddingType.PKCS7, blockSize);
            var mode = ModeFactory.CreateEncryptionMode(encryptionMode);
            var loki97V2 = new Loki97Impl(new Encryption(), new BlockPacker(), new KeyGen(), keyBuffer);

            if (doEncrypt)
            {
                inputBuffer = padder.PadBuffer(inputBuffer);
                blocksList = GetBlocksList(inputBuffer);
                
                return mode.Encrypt(loki97V2, blocksList, initializationVector);
            } 
            else
            {
                blocksList = GetBlocksList(inputBuffer);
                
                return padder.RemovePadding(mode.Decrypt(loki97V2, blocksList, initializationVector));
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