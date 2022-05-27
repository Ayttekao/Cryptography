using System;
using System.Collections.Generic;
using CourseWork.LOKI97.Algorithm;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public sealed class OFB : EncryptionModeBase
    {
        public override Byte[] Encrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            var outputBuffer = new Byte[blocksList.Count * blockSize];

            var step = 0;
            var encBlock = iv;

            foreach (var block in blocksList)
            {
                encBlock = cipherAlgorithm.BlockEncrypt(encBlock, 0);
                var res = Xor(encBlock, block);

                Array.Copy(res, 0, outputBuffer, (step++) * blockSize, blockSize);
            }

            return outputBuffer;
        }

        public override Byte[] Decrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            return Encrypt(cipherAlgorithm, blocksList, iv);
        }
    }
}