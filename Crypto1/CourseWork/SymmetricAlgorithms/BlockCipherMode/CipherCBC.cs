using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.Stuff;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;

namespace CourseWork.SymmetricAlgorithms.BlockCipherMode
{
    public sealed class CipherCBC : CipherTemplate
    {
        private ICipherAlgorithm _cipherAlgorithm;
        
        public CipherCBC(ICipherAlgorithm cipherAlgorithm)
        {
            _cipherAlgorithm = cipherAlgorithm;
        }
        protected override Byte[] EncryptBlocks(List<Byte[]> blocksList, ref Byte[] iv)
        {
            var blockSize = _cipherAlgorithm.GetBlockSize();
            var outputBuffer = new Byte[blocksList.Count * blockSize];

            var step = 0;

            foreach (var block in blocksList)
            {
                var temp = Utils.Xor(iv, block);
                iv = _cipherAlgorithm.BlockEncrypt(temp, 0);
                Array.Copy(iv, 0, outputBuffer, step++ * blockSize, blockSize);
            }

            return outputBuffer;
        }

        protected override Byte[] DecryptBlocks(List<Byte[]> blocksList, ref Byte[] iv)
        {
            blocksList.Insert(0, iv);
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count - 1).ToList();
                    
            Parallel.For(0, outputBuffer.Count, index =>
                    
                outputBuffer[index] = Utils.Xor(blocksList[index], _cipherAlgorithm.BlockDecrypt(blocksList[index + 1], 0))
            );

            iv = blocksList.Last();
            return outputBuffer.SelectMany(x => x).ToArray();
        }
    }
}