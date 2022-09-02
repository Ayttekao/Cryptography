using System;
using System.Collections.Generic;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;

namespace CourseWork.SymmetricAlgorithms.AlgorithmService.Modes
{
    public abstract class EncryptionModeBase
    {
        // abstract property or get from algo
        public abstract Byte[] Encrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv);
        public abstract Byte[] Decrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv);

        // SOLID
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