using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public class ECB : EncryptionModeBase
    {
        public override byte[] Encrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var encoder = new Encoder();

            Parallel.For(0, outputBuffer.Count, counter =>
                
                outputBuffer[counter] = encoder.BlockEncrypt(blocksList[counter], 0, key)
            );

            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public override byte[] Decrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var decoder = new Decoder();

            Parallel.For(0, outputBuffer.Count, counter =>
                
                outputBuffer[counter] = decoder.BlockDecrypt(blocksList[counter], 0, key)
            );

            return outputBuffer.SelectMany(x => x).ToArray();
        }
    }
}