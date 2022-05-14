using System;
using System.Collections.Generic;
using CourseWork.LOKI97.AlgorithmService.Enums;
using CourseWork.LOKI97.AlgorithmService.Modes;

namespace CourseWork.LOKI97.AlgorithmService
{
    public class AlgorithmService
    {
        static readonly int blockSize = 16;

        public static byte[] LaunchAlgorithm(byte[] inputBuffer, byte[] keyBuffer, byte[] initializationVector, EncryptionMode encryptionMode, bool doEncrypt)
        {
            KeyGeneration keyGeneration = new KeyGeneration();
            int blocksQuantity;
            List<byte[]> blocksArray;
    
            if (inputBuffer.Length % blockSize == 0)
            {
                blocksQuantity = inputBuffer.Length / 16;
            }
            else
            {
                blocksQuantity = inputBuffer.Length / 16 + 1;
            }
    
            blocksArray = new List<byte[]>(blocksQuantity);
    
            for (int i = 0; i < blocksQuantity; i++)
            {
                int step = i * blockSize;
                blocksArray.Add(CopyOfRange(inputBuffer, step, blockSize + step));
            }
    
            Object key = keyGeneration.MakeKey(keyBuffer);
    
            var mode = ModeFactory.CreateEncryptionMode(encryptionMode);
    
                if (doEncrypt)
                {
                    return mode.Encrypt(blocksArray, key, initializationVector);
                } 
                else
                {
                var badCharCount = 0;
                byte[] outputBuffer = mode.Decrypt(blocksArray, key, initializationVector);
    
                for (int i = 0; i < 15; i++)
                {
                    if (outputBuffer[outputBuffer.Length - i - 1] == 0)
                        badCharCount++;
                    else
                        break;
                }
                
                return CopyOfRange(outputBuffer, 0, outputBuffer.Length - badCharCount);
            }
        }

        static byte[] CopyOfRange (byte[] src, int start, int end) {
            int len = end - start;
            byte[] dest = new byte[len];
            for (var i = 0; i < len; i++)
            {
                dest[i] = src[start + i]; // so 0..n = 0+x..n+x
            }
            return dest;
        }
    }
}