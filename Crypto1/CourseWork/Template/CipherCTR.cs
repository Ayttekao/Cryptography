using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;
using CourseWork.Stuff;

namespace CourseWork.Template
{
    public class CipherCTR : CipherTemplate
    {
        private ICipherAlgorithm _cipherAlgorithm;
        
        public CipherCTR(ICipherAlgorithm cipherAlgorithm)
        {
            _cipherAlgorithm = cipherAlgorithm;
        }
        
        protected override Byte[] EncryptBlocks(List<Byte[]> blocksList, ref Byte[] iv)
        {
            var blockSize = _cipherAlgorithm.GetBlockSize();
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();

            var value = iv;
            Parallel.For(0, outputBuffer.Count, index =>
            {
                outputBuffer[index] = Utils.Xor
                (
                    blocksList[index],
                    _cipherAlgorithm.BlockEncrypt(Utils.IncrementCounterByOne(value, blockSize), 0)
                );
            });
            iv = value;

            return outputBuffer.SelectMany(x => x).ToArray();
        }

        protected override Byte[] DecryptBlocks(List<Byte[]> blocksList, ref Byte[] iv)
        {
            return EncryptBlocks(blocksList, ref iv);
        }
    }
}