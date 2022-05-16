using System;
using System.Collections.Generic;
using CourseWork.LOKI97.Algorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public sealed class OFB : EncryptionModeBase
    {
        public override byte[] Encrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            var outputBuffer = new byte[blocksList.Count * blockSize];
            var encoder = new Encoder();

            var step = 0;
            var encBlock = iv;

            foreach (var block in blocksList)
            {
                encBlock = encoder.BlockEncrypt(encBlock, 0, key);
                var res = Xor(encBlock, block);

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