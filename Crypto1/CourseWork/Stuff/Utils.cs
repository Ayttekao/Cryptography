using System;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;

namespace CourseWork.Stuff
{
    internal class Utils
    {
        public static Byte[] Xor(Byte[] a, Byte[] b)
        {
            Byte[] res = new Byte[a.Length];

            for (var i = 0; i < a.Length; i++)
            {
                res[i] = (Byte)(a[i] ^ b[i]);
            }

            return res;
        }

        public static Byte[] IncrementCounterByOne(Byte[] initCounterValue, Int32 blockSize)
        {
            Byte[] tmp = (Byte[])initCounterValue.Clone();
            for (var i = blockSize; i > 0; i--)
            {
                tmp[i - 1]++;
                if (tmp[i - 1] != 0)
                {
                    break;
                }
            }

            return tmp;
        }

        public static Boolean IsWrongInit(ICipherAlgorithm cipherAlgorithm, Byte[] iv, Byte[] valueForHash,
            Byte[] hashedValue)
        {
            var initial = GetInitial(iv, cipherAlgorithm.GetBlockSize());
            var hashAlgorithm = MD5.Create();

            return !cipherAlgorithm.BlockEncrypt(Xor(initial,
                        hashAlgorithm.ComputeHash(valueForHash)),
                    0)
                .SequenceEqual(hashedValue);
        }

        public static BigInteger GetDeltaAsBiginteger(Byte[] iv, Int32 blockSize)
        {
            return new BigInteger(GetDelta(iv, blockSize));
        }

        public static BigInteger GetInitialAsBiginteger(Byte[] iv, Int32 blockSize)
        {
            return new BigInteger(GetInitial(iv, blockSize));
        }

        public static Byte[] GetDelta(Byte[] iv, Int32 blockSize)
        {
            var deltaArr = new Byte[blockSize];
            Array.Copy(iv, blockSize, deltaArr, 0, blockSize);
            return deltaArr;
        }

        public static Byte[] GetInitial(Byte[] iv, Int32 blockSize)
        {
            var initial = new Byte[blockSize];
            Array.Copy(iv, 0, initial, 0, blockSize);
            return initial;
        }
    }
}