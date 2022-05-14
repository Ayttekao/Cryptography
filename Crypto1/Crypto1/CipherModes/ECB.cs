using System;
using System.Linq;
using System.Threading.Tasks;
using Crypto1.CypherAlgorithm;
using Crypto1.Padding;

namespace Crypto1.CipherModes
{
    public class ECB : CipherModeBase
    {
        public ECB(ICypherAlgorithm algorithm, Byte[] initializationVector, PaddingType paddingType, Int32 blockSize) :
            base(algorithm, initializationVector, paddingType, blockSize) { }

        public override Byte[] Encrypt(Byte[] inputBlock)
        {
            var result = Stuffer.PadBuffer(inputBlock);
            var blocks = InitList(result.Length);
            var blockList = GetListFromArray(result);
                    
            Parallel.For(0, result.Length / BlockSize, count =>
                    
                blocks[count] = Algorithm.Encrypt(blockList[count])
            );
            
            return blocks.SelectMany(x => x).ToArray();
        }

        public override Byte[] Decrypt(Byte[] inputBlock)
        {
            var blocks = InitList(inputBlock.Length);
            var blockList = GetListFromArray(inputBlock);
                    
            Parallel.For(0, inputBlock.Length / BlockSize, i =>
                    
                blocks[i] = Algorithm.Decrypt(blockList[i])
            );
            
            return Stuffer.RemovePadding(blocks);
        }
    }
}