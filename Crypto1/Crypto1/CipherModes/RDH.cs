using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crypto1.CypherAlgorithm;

namespace Crypto1.CipherModes
{
    public class RDH : CipherModeBase
    {
        private readonly String _valueForHash;

        public RDH(ICypherAlgorithm algorithm, byte[] initializationVector, String valueForHash) 
            : base(algorithm, initializationVector)
        {
            _valueForHash = valueForHash;
        }

        public override byte[] Encrypt(byte[] inputBlock)
        {
            var result = PadBuffer(inputBlock);
            var blocks = InitList(result.Length);
            var deltaArr = new byte[8];
            Array.Copy(InitializationVector, 8, deltaArr, 0, BlockSize);
            var copyInitializationVector = new byte[8];
            Array.Copy(InitializationVector, 0, copyInitializationVector, 0, BlockSize);
            var initializationVector = BitConverter.ToUInt64(copyInitializationVector, 0);
            var delta = BitConverter.ToUInt64(deltaArr, 0);
            blocks.Add(null);
            blocks[0] = Algorithm.Encrypt(copyInitializationVector);
            blocks.Add(null);
            blocks[1] = Xor(copyInitializationVector, PadBuffer(BitConverter.GetBytes(_valueForHash.GetHashCode())));
                    
                    
            var blockList = GetListFromArray(result);
            var counterList = new List<byte[]>();
            for (var count = 0; count < result.Length / BlockSize; count++)
            {
                initializationVector += delta;
                copyInitializationVector = BitConverter.GetBytes(initializationVector);
                counterList.Add(copyInitializationVector);
            }
                    
            Parallel.For(0, result.Length / BlockSize, i =>
                    
                blocks[i + 2] = Algorithm.Encrypt(Xor(counterList[i], blockList[i])) 
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
                    
            if (!Xor(
                    copyInitializationVector, 
                    PadBuffer(BitConverter.GetBytes(_valueForHash.GetHashCode()))
                    ).SequenceEqual(blockList[1])
                )
            {
                throw new ArgumentException();
            }
                    
            var counterList = new List<byte[]>();
            for (var count = 0; count < inputBlock.Length / BlockSize - 2; count++)
            {
                initializationVector += delta;
                copyInitializationVector = BitConverter.GetBytes(initializationVector);
                counterList.Add(copyInitializationVector);
            }
                    
            Parallel.For(2, inputBlock.Length / BlockSize, i =>
                    
                blocks[i - 2] = Xor(Algorithm.Decrypt(blockList[i]), counterList[i - 2])
            );
            blocks.RemoveAt(blocks.Count - 1);
            blocks.RemoveAt(blocks.Count - 1);
            
            return RemovePadding(blocks);
        }
    }
}