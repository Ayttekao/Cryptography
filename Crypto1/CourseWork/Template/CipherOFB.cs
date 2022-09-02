using System;
using System.Collections.Generic;
using CourseWork.Stuff;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;

namespace CourseWork.Template
{
    public sealed class CipherOFB : CipherTemplate
    {
        private ICipherAlgorithm _cipherAlgorithm;

        public CipherOFB(ICipherAlgorithm cipherAlgorithm)
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
                iv = _cipherAlgorithm.BlockEncrypt(iv, 0);
                var res = Utils.Xor(iv, block);
                Array.Copy(res, 0, outputBuffer, step++ * blockSize, blockSize);
            }

            return outputBuffer;
        }

        protected override Byte[] DecryptBlocks(List<Byte[]> blocksList, ref Byte[] iv)
        {
            return EncryptBlocks(blocksList, ref iv);
        }
    }
}