using System;
using System.Collections.Generic;
using CourseWork.LOKI97.Algorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public class ECB : EncryptionModeBase
    {
        public override byte[] Encrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            byte[] outputBuffer = new byte[blocksList.Count * blockSize];
            Encoder encoder = new Encoder();

            int step = 0;
            foreach (var block in blocksList)
            {
                byte[] temp = encoder.BlockEncrypt(block, 0, key);

                Array.Copy(temp, 0, outputBuffer, (step++) * blockSize, blockSize);
            }

            return outputBuffer;
        }

        public override byte[] Decrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            byte[] outputBuffer = new byte[blocksList.Count * blockSize];
            Decoder decoder = new Decoder();

            int step = 0;
            foreach (var block in blocksList)
            {
                byte[] temp = decoder.BlockDecrypt(block, 0, key);

                Array.Copy(temp, 0, outputBuffer, (step++) * blockSize, blockSize);
            }

            return outputBuffer;
        }
    }
}