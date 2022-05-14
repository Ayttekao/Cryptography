using System;
using System.Linq;
using Crypto1.CipherAlgorithm;
using Crypto1.Padding;

namespace Crypto1.CipherModes
{
    public class OFB : CipherModeBase
    {
        public OFB(ICipherAlgorithm algorithm, Byte[] initializationVector, PaddingType paddingType, Int32 blockSize) :
            base(algorithm, initializationVector, paddingType, blockSize) { }

        public override Byte[] Encrypt(Byte[] inputBlock)
        {
            var result = Stuffer.PadBuffer(inputBlock);
            var blocks = Enumerable.Repeat(default(Byte[]), result.Length / BlockSize).ToList();
            var previousBlock = new Byte[BlockSize];
            var currentBlock = new Byte[BlockSize];
            Array.Copy(InitializationVector, previousBlock, previousBlock.Length);
                    
            for (var count = 0; count < result.Length / BlockSize; count++)
            {
                Array.Copy(result, count * BlockSize, currentBlock, 0, BlockSize);
                var encryptBlock = Algorithm.Encrypt(previousBlock);
                blocks[count] = Xor(encryptBlock, currentBlock);
                Array.Copy(encryptBlock, previousBlock, BlockSize);
            }

            return blocks.SelectMany(x => x).ToArray();
        }

        public override Byte[] Decrypt(Byte[] inputBlock)
        {
            var blocks = Enumerable.Repeat(default(Byte[]), inputBlock.Length / BlockSize).ToList();
            var previousBlock = new Byte[BlockSize];
            var currentBlock = new Byte[BlockSize];
            Array.Copy(InitializationVector, previousBlock, previousBlock.Length);
                    
            for (var count = 0; count < inputBlock.Length / BlockSize; count++)
            {
                Array.Copy(inputBlock, count * BlockSize, currentBlock, 0, BlockSize);
                var encryptBlock = Algorithm.Encrypt(previousBlock);
                blocks[count] = Xor(encryptBlock, currentBlock);
                Array.Copy(encryptBlock, previousBlock, BlockSize);
            }
            
            return Stuffer.RemovePadding(blocks);
        }
    }
}