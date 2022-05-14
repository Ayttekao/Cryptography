using System;
using System.Collections.Generic;
using CourseWork.LOKI97.Algorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public class CFB : EncryptionModeBase
    {
        public override byte[] Encrypt(List<byte[]> blocksArray, object key, byte[] iv)
        {
            byte[] outputBuffer = new byte[blocksArray.Count * blockSize];
            Encoder encoder = new Encoder();

            int step = 0;
            byte[] encBlock = iv;

            foreach (var block in blocksArray)
            {
                encBlock = encoder.BlockEncrypt(encBlock, 0, key);
                encBlock = Xor(encBlock, block);

                Array.Copy(encBlock, 0, outputBuffer, (step++) * blockSize, blockSize);
            }

            return outputBuffer;
        }

        public override byte[] Decrypt(List<byte[]> blocksArray, object key, byte[] iv)
        {
            byte[] outputBuffer = new byte[blocksArray.Count * blockSize];
            Encoder encoder = new Encoder();

            int step = 0;
            byte[] encBlock = iv;

            foreach (var block in blocksArray)
            {
                encBlock = encoder.BlockEncrypt(encBlock, 0, key);

                byte[] temp = Xor(encBlock, block);
                encBlock = block;

                Array.Copy(temp, 0, outputBuffer, (step++) * blockSize, blockSize);
            }

            return outputBuffer;
        }
    }
}