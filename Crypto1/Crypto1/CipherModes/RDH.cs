using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crypto1.CipherAlgorithm;
using Crypto1.Padding;

namespace Crypto1.CipherModes
{
    public class RDH : CipherModeBase
    {
        private readonly String _valueForHash;

        public RDH(ICipherAlgorithm algorithm, Byte[] initializationVector, String valueForHash, PaddingType paddingType, Int32 blockSize) 
            : base(algorithm, initializationVector, paddingType, blockSize)
        {
            _valueForHash = valueForHash;
        }

        public override Byte[] Encrypt(Byte[] inputBlock)
        {
            var result = Stuffer.PadBuffer(inputBlock);
            var blocks = Enumerable.Repeat(default(Byte[]), result.Length / BlockSize).ToList();
            var deltaArr = new Byte[8];
            Array.Copy(InitializationVector, 8, deltaArr, 0, BlockSize);
            var copyInitializationVector = new Byte[8];
            Array.Copy(InitializationVector, 0, copyInitializationVector, 0, BlockSize);
            var initializationVector = BitConverter.ToUInt64(copyInitializationVector, 0);
            var delta = BitConverter.ToUInt64(deltaArr, 0);
            blocks.Add(null);
            blocks[0] = Algorithm.Encrypt(copyInitializationVector);
            blocks.Add(null);
            //TODO: not use getHashCode
            blocks[1] = Xor(copyInitializationVector, Stuffer.PadBuffer(BitConverter.GetBytes(_valueForHash.GetHashCode())));

            var blockList = GetListFromArray(result);
            var counterList = new List<Byte[]>();
            for (var count = 0; count < result.Length / BlockSize; count++)
            {
                initializationVector += delta;
                copyInitializationVector = BitConverter.GetBytes(initializationVector);
                counterList.Add(copyInitializationVector);
            }
                    
            Parallel.For(0, result.Length / BlockSize, i =>
                    
                blocks[i + 2] = Algorithm.Encrypt(Xor(counterList[i], blockList[i])) 
            );
            
            return blocks.SelectMany(x => x).ToArray();
        }

        public override Byte[] Decrypt(Byte[] inputBlock)
        {
            var blocks = Enumerable.Repeat(default(Byte[]), inputBlock.Length / BlockSize).ToList();
            var curBlock = new Byte[BlockSize];
            var deltaArr = new Byte[8];
            Array.Copy(InitializationVector, InitializationVector.Length / 2, deltaArr, 0, BlockSize);
            var delta = BitConverter.ToUInt64(deltaArr, 0);
            Array.Copy(inputBlock, 0, curBlock, 0, BlockSize);
            var copyInitializationVector = Algorithm.Decrypt(curBlock);
            var initializationVector = BitConverter.ToUInt64(copyInitializationVector, 0);
            var blockList = GetListFromArray(inputBlock);
                    
            if (!Xor(
                    copyInitializationVector, 
                    Stuffer.PadBuffer(BitConverter.GetBytes(_valueForHash.GetHashCode()))
                    ).SequenceEqual(blockList[1])
                )
            {
                throw new ArgumentException(nameof(_valueForHash));
            }
                    
            var counterList = new List<Byte[]>();
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
            
            return Stuffer.RemovePadding(blocks);
        }
    }
}