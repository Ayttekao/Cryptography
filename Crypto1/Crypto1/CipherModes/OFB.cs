using System;
using Crypto1.CypherAlgorithm;

namespace Crypto1.CipherModes
{
    public class OFB : CipherModeBase
    {
        public OFB(ICypherAlgorithm algorithm, byte[] initializationVector) : base(algorithm, initializationVector) { }

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
                var encryptBlock = Algorithm.Encrypt(previousBlock);
                blocks[count] = Xor(encryptBlock, currentBlock);
                Array.Copy(encryptBlock, previousBlock, BlockSize);
            }

            return ConvertListToArray(blocks);
        }

        public override byte[] Decrypt(byte[] inputBlock)
        {
            var blocks = InitList(inputBlock.Length);
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
            
            return RemovePadding(blocks);
        }
    }
}