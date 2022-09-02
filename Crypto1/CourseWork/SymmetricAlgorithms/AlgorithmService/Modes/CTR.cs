using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;

namespace CourseWork.SymmetricAlgorithms.AlgorithmService.Modes
{
    public sealed class CTR : EncryptionModeBase
    {
        public override Byte[] Encrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            var blockSize = cipherAlgorithm.GetBlockSize();
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var initCounterValue = iv;

            Parallel.For(0, outputBuffer.Count, index =>
            {
                outputBuffer[index] = Xor
                (
                    blocksList[index],
                    cipherAlgorithm.BlockEncrypt(IncrementCounterByOne(initCounterValue, blockSize), 0)
                );
            });

            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public override Byte[] Decrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            return Encrypt(cipherAlgorithm, blocksList, iv);
        }

        private Byte[] IncrementCounterByOne(Byte[] initCounterValue, Int32 blockSize)
        {
            Byte[] tmp = (Byte[])initCounterValue.Clone();
            for (var i = blockSize; i > 0; i--)
            {
                tmp[i - 1]++;
                if (tmp[i - 1] != 0)
                {
                    break;
                }
            }

            return tmp;
        }
    }
}