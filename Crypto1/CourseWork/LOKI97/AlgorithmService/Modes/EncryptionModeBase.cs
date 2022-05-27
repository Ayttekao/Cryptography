using System;
using System.Collections.Generic;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public abstract class EncryptionModeBase
    {
        protected int blockSize = 16;
        public abstract Byte[] Encrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv);
        public abstract Byte[] Decrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv);

        protected static Byte[] Xor(Byte[] a, Byte[] b)
        {
            Byte[] res = new Byte[a.Length];

            for (var i = 0; i < a.Length; i++)
            {
                res[i] = (Byte) (a[i] ^ b[i]);
            }
            return res;
        }
    }
}