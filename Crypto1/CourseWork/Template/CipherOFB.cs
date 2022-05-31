using System;
using System.Collections.Generic;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;
using CourseWork.Stuff;

namespace CourseWork.Template
{
    public class CipherOFB : CipherTemplate
    {
        private ICipherAlgorithm _cipherAlgorithm;

        public CipherOFB(ICipherAlgorithm cipherAlgorithm)
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
                var res = Utils.Xor(iv, block);
                Array.Copy(res, 0, outputBuffer, step++ * blockSize, blockSize);
            }

            return outputBuffer;
        }

        protected override byte[] DecryptBlocks(List<byte[]> blocksList, ref byte[] iv)
        {
            return EncryptBlocks(blocksList, ref iv);
        }
    }
}