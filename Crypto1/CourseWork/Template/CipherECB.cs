using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;

namespace CourseWork.Template
{
    public sealed class CipherECB : CipherTemplate
    {
        private ICipherAlgorithm _cipherAlgorithm;
        
        public CipherECB(ICipherAlgorithm cipherAlgorithm)
        {
            _cipherAlgorithm = cipherAlgorithm;
        }
        
        protected override Byte[] EncryptBlocks(List<Byte[]> blocksList, ref Byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();

            Parallel.For(0, outputBuffer.Count, counter =>
                
                outputBuffer[counter] = _cipherAlgorithm.BlockEncrypt(blocksList[counter], 0)
            );

            return outputBuffer.SelectMany(x => x).ToArray();
        }

        protected override Byte[] DecryptBlocks(List<Byte[]> blocksList, ref Byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();

            Parallel.For(0, outputBuffer.Count, counter =>
                
                outputBuffer[counter] = _cipherAlgorithm.BlockDecrypt(blocksList[counter], 0)
            );

            return outputBuffer.SelectMany(x => x).ToArray();
        }
    }
}