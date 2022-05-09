using System;
using System.Threading.Tasks;
using Crypto1.CypherAlgorithm;

namespace Crypto1.CipherModes
{
    public class CBC : CipherModeBase
    {
        public CBC(ICypherAlgorithm algorithm, Byte[] initializationVector) : base(algorithm, initializationVector) { }
        
        public override byte[] Encrypt(byte[] inputBlock)
        {
            var result = PadBuffer(inputBlock);
            var blocks = InitList(result.Length);
            var previousBlock = new Byte[BlockSize];
            var currentBlock = new Byte[BlockSize];
            Array.Copy(InitializationVector, previousBlock, previousBlock.Length);
                    
            for (var count = 0; count < result.Length / BlockSize; count++)
            {
                Array.Copy(result, count * BlockSize, currentBlock, 0, BlockSize);
                blocks[count] = Algorithm.Encrypt(Xor(currentBlock, previousBlock));
                Array.Copy(blocks[count], previousBlock, BlockSize);
            }
            
            return ConvertListToArray(blocks);
        }

        public override byte[] Decrypt(byte[] inputBlock)
        {
            var blocks = InitList(inputBlock.Length);
            var blockList = GetListFromArray(inputBlock);
            blockList.Insert(0, InitializationVector);
                    
            Parallel.For(0, inputBlock.Length / BlockSize, i =>
                    
                blocks[i] = Xor(blockList[i], Algorithm.Decrypt(blockList[i + 1]))
            );
            
            return RemovePadding(blocks);
        }
    }
}