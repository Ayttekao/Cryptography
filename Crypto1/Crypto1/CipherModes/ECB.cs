using System.Threading.Tasks;
using Crypto1.CypherAlgorithm;

namespace Crypto1.CipherModes
{
    public class ECB : CipherModeBase
    {
        public ECB(ICypherAlgorithm algorithm, byte[] initializationVector) : base(algorithm, initializationVector) { }

        public override byte[] Encrypt(byte[] inputBlock)
        {
            var result = PadBuffer(inputBlock);
            var blocks = InitList(result.Length);
            var blockList = GetListFromArray(result);
                    
            Parallel.For(0, result.Length / BlockSize, count =>
                    
                blocks[count] = Algorithm.Encrypt(blockList[count])
            );
            
            return ConvertListToArray(blocks);
        }

        public override byte[] Decrypt(byte[] inputBlock)
        {
            var blocks = InitList(inputBlock.Length);
            var blockList = GetListFromArray(inputBlock);
                    
            Parallel.For(0, inputBlock.Length / BlockSize, i =>
                    
                blocks[i] = Algorithm.Decrypt(blockList[i])
            );
            
            return RemovePadding(blocks);
        }
    }
}