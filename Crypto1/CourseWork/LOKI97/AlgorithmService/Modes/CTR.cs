using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public sealed class CTR : EncryptionModeBase
    {
        public override byte[] Encrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var encoder = new Encoder();
            var initCounterValue = iv;

            Parallel.For(0, outputBuffer.Count, index =>
            {
                outputBuffer[index] = Xor
                (
                    blocksList[index],
                    encoder.BlockEncrypt(IncrementCounterByOne(initCounterValue), 0, key)
                );
            });

            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public override byte[] Decrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            return Encrypt(blocksList, key, iv);
        }

        private Byte[] IncrementCounterByOne(Byte[] initCounterValue)
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