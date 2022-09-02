using System;
using System.Collections.Generic;
using System.Linq;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;

namespace CourseWork.SymmetricAlgorithms.AlgorithmService.Modes
{
    public sealed class OFB : EncryptionModeBase
    {
        public override Byte[] Encrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            var blockSize = cipherAlgorithm.GetBlockSize();
            var outputBuffer = new Byte[blocksList.Count * blockSize];
            var step = 0;
            var encBlock = iv.ToArray();

            foreach (var block in blocksList)
            {
                encBlock = cipherAlgorithm.BlockEncrypt(encBlock, 0);
                var res = Xor(encBlock, block);

                Array.Copy(res, 0, outputBuffer, step++ * blockSize, blockSize);
            }

            return outputBuffer;
        }

        public override Byte[] Decrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            return Encrypt(cipherAlgorithm, blocksList, iv.ToArray());
        }
    }
}