using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public sealed class CFB : EncryptionModeBase
    {
        public override Byte[] Encrypt(List<Byte[]> blocksList, object key, Byte[] iv)
        {
            var outputBuffer = new Byte[blocksList.Count * blockSize];
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

        public override Byte[] Decrypt(List<Byte[]> blocksList, object key, Byte[] iv)
        {
            var encoder = new Encoder();
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var inputBuffer = new List<Byte[]>(blocksList);
            inputBuffer.Insert(0, iv);

            Parallel.For(0, blocksList.Count, index =>
                    
                outputBuffer[index] = Xor(encoder.BlockEncrypt(inputBuffer[index], 0, key), inputBuffer[index + 1])
            );
            
            return outputBuffer.SelectMany(x => x).ToArray();
        }
    }
}