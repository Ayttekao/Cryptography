using System;
using System.Collections.Generic;
using CourseWork.LOKI97.Algorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public class CBC : EncryptionModeBase
    {
        public override byte[] Encrypt(List<byte[]> blocksList, Object key, byte[] iv)
        {
            byte[] outputBuffer = new byte[blocksList.Count * blockSize];
            Encoder encoder = new Encoder();

            int step = 0;
            byte[] encBlock = iv;

            foreach (var block in blocksList)
            {
                byte[] temp = Xor(encBlock, block);

                encBlock = encoder.BlockEncrypt(temp, 0, key);

                Array.Copy(encBlock, 0, outputBuffer, step++ * blockSize, blockSize);
            }

            return outputBuffer;
        }

        public override byte[] Decrypt(List<byte[]> blocksList, Object key, byte[] iv)
        {
            byte[] outputBuffer = new byte[blocksList.Count * blockSize];
            Decoder decoder = new Decoder();

            int step = 0;
            byte[] decBlock;
            byte[] encBlock = iv;

            foreach (var block in blocksList)
            {
                decBlock = decoder.BlockDecrypt(block, 0, key);

                byte[] temp = EncryptionModeBase.Xor(decBlock, encBlock);
                encBlock = block; // !

                Array.Copy(temp, 0, outputBuffer, (step++) * blockSize, blockSize);
            }

            return outputBuffer;
        }
    }
}