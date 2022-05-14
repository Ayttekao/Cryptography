using System;
using System.Collections.Generic;
using CourseWork.LOKI97.Algorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public class OFB : EncryptionModeBase
    {
        public override byte[] Encrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            byte[] outputBuffer = new byte[blocksList.Count * blockSize];
            Encoder encoder = new Encoder();

            int step = 0;
            byte[] encBlock = iv;

            foreach (var block in blocksList)
            {
                encBlock = encoder.BlockEncrypt(encBlock, 0, key);
                byte[] res = Xor(encBlock, block);

                Array.Copy(res, 0, outputBuffer, (step++) * blockSize, blockSize);
            }

            return outputBuffer;
        }

        public override byte[] Decrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            return Encrypt(blocksList, key, iv);
        }
    }
}