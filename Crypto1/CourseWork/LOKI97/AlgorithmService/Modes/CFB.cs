using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public class CFB : EncryptionModeBase
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
                encBlock = Xor(encBlock, block);

                Array.Copy(encBlock, 0, outputBuffer, (step++) * blockSize, blockSize);
            }

            return outputBuffer;
        }

        public override byte[] Decrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var encoder = new Encoder();

            var encBlock = iv;
            
            Parallel.For(0, outputBuffer.Count, counter =>
                {
                    encBlock = encoder.BlockEncrypt(encBlock, 0, key);

                    var temp = Xor(encBlock, blocksList[counter]);
                    encBlock = blocksList[counter];
                    outputBuffer[counter] = temp;
                }
            );

            return outputBuffer.SelectMany(x => x).ToArray();
        }
    }
}