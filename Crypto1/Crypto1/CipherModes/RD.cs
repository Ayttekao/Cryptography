using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crypto1.CypherAlgorithm;

namespace Crypto1.CipherModes
{
    public class RD : CipherModeBase
    {
        public RD(ICypherAlgorithm algorithm, byte[] initializationVector) : base(algorithm, initializationVector) { }

        public override byte[] Encrypt(byte[] inputBlock)
        {
            var result = PadBuffer(inputBlock);
            var blocks = InitList(result.Length);
            var deltaArr = new byte[8];
            Array.Copy(InitializationVector, 8, deltaArr, 0, BlockSize);
            var delta = BitConverter.ToUInt64(deltaArr, 0);
            var copyInitializationVector = new byte[8];
            Array.Copy(InitializationVector, 0, copyInitializationVector, 0, BlockSize);
            var initializationVector = BitConverter.ToUInt64(copyInitializationVector, 0);
            blocks.Add(null);
            blocks[0] = Algorithm.Encrypt(copyInitializationVector);
                    
            var blockList = GetListFromArray(result);
            var counterList = new List<byte[]>();
            for (var count = 0; count < result.Length / BlockSize; count++)
            {
                counterList.Add(copyInitializationVector);
                initializationVector += delta;
                copyInitializationVector = BitConverter.GetBytes(initializationVector);
            }
                    
            Parallel.For(0, result.Length / BlockSize, count =>
                    
                blocks[count + 1] = Algorithm.Encrypt(Xor(counterList[count], blockList[count])) 
            );
            
            return ConvertListToArray(blocks);
        }

        public override byte[] Decrypt(byte[] inputBlock)
        {
            var blocks = InitList(inputBlock.Length);
            var curBlock = new byte[BlockSize];
            var deltaArr = new byte[8];
            Array.Copy(InitializationVector, InitializationVector.Length / 2, deltaArr, 0, BlockSize);
            var delta = BitConverter.ToUInt64(deltaArr, 0);
            Array.Copy(inputBlock, 0, curBlock, 0, BlockSize);
            var copyInitializationVector = Algorithm.Decrypt(curBlock);
            var initializationVector = BitConverter.ToUInt64(copyInitializationVector, 0);
            var blockList = GetListFromArray(inputBlock);
            var counterList = new List<byte[]>();
                    
            for (var count = 0; count < inputBlock.Length / BlockSize; count++)
            {
                counterList.Add(copyInitializationVector);
                initializationVector += delta;
                copyInitializationVector = BitConverter.GetBytes(initializationVector);
            }
                    
            Parallel.For(1, inputBlock.Length / BlockSize, count =>
                    
                blocks[count - 1] = Xor(Algorithm.Decrypt(blockList[count]), counterList[count - 1]) 
            );
            blocks.RemoveAt(blocks.Count - 1);
            
            return RemovePadding(blocks);
        }
    }
}