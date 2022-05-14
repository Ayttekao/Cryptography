using System;
using System.Collections.Generic;
using Crypto1.CipherAlgorithm;
using Crypto1.Padding;

namespace Crypto1.CipherModes
{
    public abstract class CipherModeBase
    {
        protected static Int32 BlockSize;
        protected Padder Stuffer;
        protected readonly Byte[] InitializationVector;
        protected ICipherAlgorithm Algorithm { get; }
        public abstract Byte[] Encrypt(Byte[] inputBlock);
        public abstract Byte[] Decrypt(Byte[] inputBlock);

        protected CipherModeBase(ICipherAlgorithm algorithm,
            Byte[] initializationVector,
            PaddingType paddingType,
            Int32 blockSize)
        {
            InitializationVector = initializationVector;
            Algorithm = algorithm;
            BlockSize = blockSize;
            Stuffer = new Padder(paddingType, blockSize);
        }
        
        protected static List<Byte[]> GetListFromArray(Byte[] result)
        {
            var resultList = new List<Byte[]>();

            for (var i = 0; i < result.Length / BlockSize; i++)
            {
                resultList.Add(new Byte[BlockSize]);
                for (var j = 0; j < BlockSize; j++)
                {
                    resultList[i][j] = result[BlockSize * i + j];
                }
            }

            return resultList;
        }

        protected static Byte[] Xor(Byte[] left, Byte[] right)
        {
            var result = new Byte[left.Length];
            for (var count = 0; count < left.Length; count++)
            {
                result[count] = (Byte)(left[count] ^ right[count]);
            }
            return result;
        }
    }
}