using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public sealed class CBC : EncryptionModeBase
    {
        public override byte[] Encrypt(List<byte[]> blocksList, Object key, byte[] iv)
        {
            var outputBuffer = new byte[blocksList.Count * blockSize];
            var encoder = new Encoder();

            var step = 0;
            var encBlock = iv;

            foreach (var block in blocksList)
            {
                var temp = Xor(encBlock, block);
                encBlock = encoder.BlockEncrypt(temp, 0, key);
                Array.Copy(encBlock, 0, outputBuffer, step++ * blockSize, blockSize);
            }

            return outputBuffer;
        }

        public override byte[] Decrypt(List<byte[]> blocksList, Object key, byte[] iv)
        {
            var decoder = new Decoder();
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var inputBuffer = new List<Byte[]>(blocksList);
            inputBuffer.Insert(0, iv);
                    
            Parallel.For(0, blocksList.Count, index =>
                    
                outputBuffer[index] = Xor(inputBuffer[index], decoder.BlockDecrypt(inputBuffer[index + 1], 0, key))
            );
            
            return outputBuffer.SelectMany(x => x).ToArray();
        }
    }
}