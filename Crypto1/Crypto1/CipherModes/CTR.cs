using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crypto1.CypherAlgorithm;

namespace Crypto1.CipherModes
{
    public class CTR : CipherModeBase
    {
        public CTR(ICypherAlgorithm algorithm, byte[] initializationVector) : base(algorithm, initializationVector) { }

        public override byte[] Encrypt(byte[] inputBlock)
        {
            var result = PadBuffer(inputBlock);
            var blocks = InitList(result.Length);
            var copyInitializationVector = new byte[8];
            InitializationVector.CopyTo(copyInitializationVector, 0);
            var counter = BitConverter.ToUInt64(copyInitializationVector, 0);
            var blockList = GetListFromArray(result);
            var counterList = new List<byte[]>();
            for (var count = 0; count < result.Length / BlockSize; count++)
            {
                counterList.Add(copyInitializationVector);
                counter++;
                copyInitializationVector = BitConverter.GetBytes(counter);
            }
                    
            Parallel.For(0, result.Length / BlockSize, count =>
                    
                blocks[count] =  Xor(Algorithm.Encrypt(counterList[count]), blockList[count])
            );
            
            return ConvertListToArray(blocks);
        }

        public override byte[] Decrypt(byte[] inputBlock)
        {
            var blocks = InitList(inputBlock.Length);
            var copyInitializationVector = new Byte[BlockSize];
            InitializationVector.CopyTo(copyInitializationVector, 0);
            var counter = BitConverter.ToUInt64(InitializationVector, 0);
            var blockList = GetListFromArray(inputBlock);
            var counterList = new List<byte[]>();
                    
            for (var count = 0; count < inputBlock.Length / BlockSize; count++)
            {
                counterList.Add(copyInitializationVector);
                counter++;
                copyInitializationVector = BitConverter.GetBytes(counter);
            }
                    
            Parallel.For(0, inputBlock.Length / BlockSize, i =>
                    
                blocks[i] =  Xor(Algorithm.Encrypt(counterList[i]), blockList[i])
            );

            return RemovePadding(blocks);
        }
    }
}