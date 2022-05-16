using System;
using System.Collections.Generic;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public abstract class EncryptionModeBase
    {
        protected int blockSize = 16;
        public abstract byte[] Encrypt(List<byte[]> blocksList, Object key, byte[] iv);
        public abstract byte[] Decrypt(List<byte[]> blocksList, Object key, byte[] iv);

        protected static byte[] Xor(byte[] a, byte[] b)
        {
            byte[] res = new byte[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                res[i] = (byte) (a[i] ^ b[i]);
            }
            return res;
        }
    }
}