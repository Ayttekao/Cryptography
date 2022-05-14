using System;
using System.Collections.Generic;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public class ECB : EncryptionModeBase
    {
        public override byte[] Encrypt(List<byte[]> blocksArray, object key, byte[] iv)
        {
            byte[] outputBuffer = new byte[blocksArray.Count * blockSize];
            Encoder encoder = new Encoder();

            int step = 0;
            foreach (var block in blocksArray)
            {
                byte[] temp = encoder.BlockEncrypt(block, 0, key);

                Array.Copy(temp, 0, outputBuffer, (step++) * blockSize, blockSize);
            }

            return outputBuffer;
        }

        public override byte[] Decrypt(List<byte[]> blocksArray, object key, byte[] iv)
        {
            byte[] outputBuffer = new byte[blocksArray.Count * blockSize];
            Decoder decoder = new Decoder();

            int step = 0;
            foreach (var block in blocksArray)
            {
                byte[] temp = decoder.BlockDecrypt(block, 0, key);

                Array.Copy(temp, 0, outputBuffer, (step++) * blockSize, blockSize);
            }

            return outputBuffer;
        }
    }
}