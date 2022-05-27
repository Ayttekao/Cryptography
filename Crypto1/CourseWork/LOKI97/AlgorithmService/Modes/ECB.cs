using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public sealed class ECB : EncryptionModeBase
    {
        public override Byte[] Encrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();

            Parallel.For(0, outputBuffer.Count, counter =>
                
                outputBuffer[counter] = cipherAlgorithm.BlockEncrypt(blocksList[counter], 0)
            );

            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public override Byte[] Decrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();

            Parallel.For(0, outputBuffer.Count, counter =>
                
                outputBuffer[counter] = cipherAlgorithm.BlockDecrypt(blocksList[counter], 0)
            );

            return outputBuffer.SelectMany(x => x).ToArray();
        }
    }
}