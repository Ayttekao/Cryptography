using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;

namespace CourseWork.SymmetricAlgorithms.AlgorithmService.Modes
{
    public sealed class CBC : EncryptionModeBase
    {
        // передача iv по ссылке
        public override Byte[] Encrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            var blockSize = cipherAlgorithm.GetBlockSize();
            var outputBuffer = new Byte[blocksList.Count * blockSize];

            var step = 0;
            var encBlock = iv;

            foreach (var block in blocksList)
            {
                var temp = Xor(encBlock, block);
                encBlock = cipherAlgorithm.BlockEncrypt(temp, 0);
                Array.Copy(encBlock, 0, outputBuffer, step++ * blockSize, blockSize);
            }

            return outputBuffer;
        }

        public override Byte[] Decrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var inputBuffer = new List<Byte[]>(blocksList);
            inputBuffer.Insert(0, iv);
                    
            Parallel.For(0, blocksList.Count, index =>
                    
                outputBuffer[index] = Xor(inputBuffer[index], cipherAlgorithm.BlockDecrypt(inputBuffer[index + 1], 0))
            );
            
            return outputBuffer.SelectMany(x => x).ToArray();
        }
    }
}