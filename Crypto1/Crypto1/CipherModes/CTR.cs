using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crypto1.CipherAlgorithm;
using Crypto1.Padding;

namespace Crypto1.CipherModes
{
    public class CTR : CipherModeBase
    {
        public CTR(ICipherAlgorithm algorithm, Byte[] initializationVector, PaddingType paddingType, Int32 blockSize) :
            base(algorithm, initializationVector, paddingType, blockSize) { }

        public override Byte[] Encrypt(Byte[] inputBlock)
        {
            var result = Stuffer.PadBuffer(inputBlock);
            var blocks = Enumerable.Repeat(default(Byte[]), result.Length / BlockSize).ToList();
            var copyInitializationVector = new Byte[BlockSize];
            InitializationVector.CopyTo(copyInitializationVector, 0);
            var counter = BitConverter.ToUInt64(copyInitializationVector, 0);
            var blockList = GetListFromArray(result);
            var counterList = new List<Byte[]>();
            for (var count = 0; count < result.Length / BlockSize; count++)
            {
                counterList.Add(copyInitializationVector);
                counter++;
                copyInitializationVector = BitConverter.GetBytes(counter);
            }
                    
            Parallel.For(0, result.Length / BlockSize, count =>
                    
                blocks[count] = Xor(Algorithm.Encrypt(counterList[count]), blockList[count])
            );
            
            return blocks.SelectMany(x => x).ToArray();
        }

        public override Byte[] Decrypt(Byte[] inputBlock)
        {
            var blocks = Enumerable.Repeat(default(Byte[]), inputBlock.Length / BlockSize).ToList();
            var copyInitializationVector = new Byte[BlockSize];
            InitializationVector.CopyTo(copyInitializationVector, 0);
            var counter = BitConverter.ToUInt64(InitializationVector, 0);
            var blockList = GetListFromArray(inputBlock);
            var counterList = new List<Byte[]>();
                    
            for (var count = 0; count < inputBlock.Length / BlockSize; count++)
            {
                counterList.Add(copyInitializationVector);
                counter++;
                copyInitializationVector = BitConverter.GetBytes(counter);
            }
                    
            Parallel.For(0, inputBlock.Length / BlockSize, i =>
                    
                blocks[i] = Xor(Algorithm.Encrypt(counterList[i]), blockList[i])
            );

            return Stuffer.RemovePadding(blocks);
        }
    }
}