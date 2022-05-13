using System;
using System.Linq;
using System.Threading.Tasks;
using Crypto1.CypherAlgorithm;
using Crypto1.Padding;

namespace Crypto1.CipherModes
{
    public class CFB : CipherModeBase
    {
        public CFB(ICypherAlgorithm algorithm, Byte[] initializationVector, PaddingType paddingType, Int32 blockSize) :
            base(algorithm, initializationVector, paddingType, blockSize) { }

        public override Byte[] Encrypt(Byte[] inputBlock)
        {
            var result = Stuffer.PadBuffer(inputBlock);
            var blocks = InitList(result.Length);
            var previousBlock = new Byte[BlockSize];
            var currentBlock = new Byte[BlockSize];
            Array.Copy(InitializationVector, previousBlock, previousBlock.Length);
                    
            for (var count = 0; count < result.Length / BlockSize; count++)
            {
                Array.Copy(result, count * BlockSize, currentBlock, 0, BlockSize);
                blocks[count] = Xor(Algorithm.Encrypt(previousBlock), currentBlock);
                Array.Copy(blocks[count], previousBlock, BlockSize);
            }
            
            return blocks.SelectMany(x => x.ToArray()).ToArray();
        }

        public override Byte[] Decrypt(Byte[] inputBlock)
        {
            var blocks = InitList(inputBlock.Length);
            var blockList = GetListFromArray(inputBlock);
            blockList.Insert(0, InitializationVector);
                   
            Parallel.For(0, inputBlock.Length / BlockSize, i =>
                    
                blocks[i] = Xor(Algorithm.Encrypt(blockList[i]), blockList[i + 1])
            );
            
            return Stuffer.RemovePadding(blocks);
        }
    }
}