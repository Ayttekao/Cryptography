using System;
using System.Collections.Generic;
using Crypto1.CypherAlgorithm;
using Crypto1.Padding;

namespace Crypto1.CipherModes
{
    public abstract class CipherModeBase
    {
        protected static Int32 BlockSize = 8;
        protected readonly Byte[] InitializationVector;
        protected ICypherAlgorithm Algorithm { get; }
        public abstract Byte[] Encrypt(Byte[] inputBlock);
        public abstract Byte[] Decrypt(Byte[] inputBlock);

        protected CipherModeBase(ICypherAlgorithm algorithm, Byte[] initializationVector)
        {
            InitializationVector = initializationVector;
            Algorithm = algorithm;
        }
        
        protected static List<byte[]> GetListFromArray(Byte[] result)
        {
            var resultList = new List<Byte[]>();

            for (var i = 0; i < result.Length / BlockSize; i++)
            {
                resultList.Add(new byte[BlockSize]);
                for (var j = 0; j < BlockSize; j++)
                {
                    resultList[i][j] = result[BlockSize * i + j];
                }
            }

            return resultList;
        }

        protected static Byte[] PadBuffer(Byte[] buf, Int32 padFrom, Int32 padTo, PaddingType paddingType = PaddingType.PKCS7) 
        {
            if ((padTo < buf.Length) | (padTo - padFrom > 255))
            {
                return buf;
            }
            var b = new Byte[padTo];
            Buffer.BlockCopy(buf, 0, b, 0, padFrom);
            
            for (var count = padFrom; count < padTo; count++) 
            {
                switch(paddingType) 
                {
                    case PaddingType.PKCS7:
                        b[count] = (Byte) (padTo - padFrom);
                        break;
                    case PaddingType.NONE:
                        b[count] = 0;
                        break;
                    default:
                        return buf;
                }
            }
            return b;
        }

        protected static Byte[] PadBuffer(Byte[] buf, PaddingType paddingType = PaddingType.PKCS7) {
            var extraBlock = (buf.Length % BlockSize == 0) && paddingType == PaddingType.NONE ? 0 : 1;
            return PadBuffer(buf, buf.Length, ((buf.Length / BlockSize) + extraBlock) * BlockSize, paddingType);
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

        protected static Byte[] ConvertListToArray(List<Byte[]> data)
        {
            var array = new Byte[BlockSize * data.Count];
            for (var count = 0; count < data.Count; count++)
            {
                Array.Copy(data[count], 0, array, count * BlockSize, BlockSize);
            }
            return array;
        }
        
        protected static Byte[] RemovePadding(List<Byte[]> blocks)
        {
            var array = ConvertListToArray(blocks);
            var extraBlocks = array[array.Length - 1];
            var result = new Byte[array.Length - extraBlocks];
            Array.Copy(array, result, result.Length);

            return result;
        }

        protected static List<Byte[]> InitList(Int32 size)
        {
            var blocks = new List<Byte[]>();

            for (var count = 0; count < size / BlockSize; count++)
            {
                blocks.Add(null);
            }
            
            return blocks;
        }
    }
}