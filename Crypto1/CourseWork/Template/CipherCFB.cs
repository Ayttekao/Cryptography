using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;
using CourseWork.Stuff;

namespace CourseWork.Template
{
    public class CipherCFB : CipherTemplate
    {
        private ICipherAlgorithm _cipherAlgorithm;
        
        public CipherCFB(ICipherAlgorithm cipherAlgorithm)
        {
            _cipherAlgorithm = cipherAlgorithm;
        }
        
        protected override byte[] EncryptBlocks(List<byte[]> blocksList, ref byte[] iv)
        {
            var blockSize = _cipherAlgorithm.GetBlockSize();
            var outputBuffer = new Byte[blocksList.Count * blockSize];

            var step = 0;

            foreach (var block in blocksList)
            {
                iv = _cipherAlgorithm.BlockEncrypt(iv, 0);
                iv = Utils.Xor(iv, block);

                Array.Copy(iv, 0, outputBuffer, (step++) * blockSize, blockSize);
            }
            
            return outputBuffer;
        }

        protected override byte[] DecryptBlocks(List<byte[]> blocksList, ref byte[] iv)
        {
            blocksList.Insert(0, iv);
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count - 1).ToList();
            var inputBuffer = new List<Byte[]>(blocksList);

            Parallel.For(0, outputBuffer.Count, index =>
                    
                outputBuffer[index] = Utils.Xor(_cipherAlgorithm.BlockEncrypt(inputBuffer[index], 0), inputBuffer[index + 1])
            );

            iv = blocksList.Last();
            return outputBuffer.SelectMany(x => x).ToArray();
        }
    }
}