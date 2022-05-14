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
            //byte[] outputBuffer = new byte[blocksList.Count * blockSize];
            var outputBuffer = Enumerable.Repeat(default(Byte[]), inputBlock.Length / BlockSize).ToList();
            Encoder encoder = new Encoder();

            int step = 0;
            /*foreach (var block in blocksList)
            {
                byte[] temp = encoder.BlockEncrypt(block, 0, key);

                Array.Copy(temp, 0, outputBuffer, (step++) * blockSize, blockSize);
            }*/
            
            Parallel.For(var count = 0; count < result.Length / BlockSize; count++)
            {
                
            }

            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public override byte[] Decrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            byte[] outputBuffer = new byte[blocksList.Count * blockSize];
            Decoder decoder = new Decoder();

            int step = 0;
            foreach (var block in blocksList)
            {
                byte[] temp = decoder.BlockDecrypt(block, 0, key);

                Array.Copy(temp, 0, outputBuffer, (step++) * blockSize, blockSize);
            }

            return outputBuffer;
        }
    }
}