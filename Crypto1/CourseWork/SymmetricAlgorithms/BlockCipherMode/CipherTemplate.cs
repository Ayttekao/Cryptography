using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseWork.SymmetricAlgorithms.BlockCipherMode
{
    public abstract class CipherTemplate
    {
        public Byte[] Run(List<Byte[]> blocksList, ref Byte[] iv, Int32 currentBlockNumber, Boolean doEncrypt)
        {
            var outputBuffer = new List<Byte[]>();
            if (currentBlockNumber == 0)
            {
                outputBuffer = doEncrypt
                    ? ModifyOnFirstStageEncrypt(ref blocksList, ref iv)
                    : ModifyOnFirstStageDecrypt(ref blocksList, ref iv);
            }

            outputBuffer.Add(doEncrypt
                ? EncryptBlocks(blocksList, ref iv)
                : DecryptBlocks(blocksList, ref iv));
            return outputBuffer.SelectMany(x => x).ToArray();
        }

        protected abstract Byte[] EncryptBlocks(List<Byte[]> blocksList, ref Byte[] iv);
        protected abstract Byte[] DecryptBlocks(List<Byte[]> blocksList, ref Byte[] iv);

        protected virtual List<Byte[]> ModifyOnFirstStageEncrypt(ref List<Byte[]> blocksList, ref Byte[] iv)
        {
            return new List<Byte[]>();
        }

        protected virtual List<Byte[]> ModifyOnFirstStageDecrypt(ref List<Byte[]> blocksList, ref Byte[] iv)
        {
            return new List<Byte[]>();
        }
    }
}