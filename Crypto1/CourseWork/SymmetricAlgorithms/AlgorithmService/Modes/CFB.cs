using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;

namespace CourseWork.SymmetricAlgorithms.AlgorithmService.Modes
{
    public sealed class CFB : EncryptionModeBase
    {
        public override Byte[] Encrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            var blockSize = cipherAlgorithm.GetBlockSize();
            var outputBuffer = new Byte[blocksList.Count * blockSize];

            var step = 0;
            var encBlock = iv;

            foreach (var block in blocksList)
            {
                encBlock = cipherAlgorithm.BlockEncrypt(encBlock, 0);
                encBlock = Xor(encBlock, block);

                Array.Copy(encBlock, 0, outputBuffer, (step++) * blockSize, blockSize);
            }

            return outputBuffer;
        }

        public override Byte[] Decrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var inputBuffer = new List<Byte[]>(blocksList);
            inputBuffer.Insert(0, iv);

            Parallel.For(0, blocksList.Count, index =>
                    
                outputBuffer[index] = Xor(cipherAlgorithm.BlockEncrypt(inputBuffer[index], 0), inputBuffer[index + 1])
            );
            
            return outputBuffer.SelectMany(x => x).ToArray();
        }
    }
}