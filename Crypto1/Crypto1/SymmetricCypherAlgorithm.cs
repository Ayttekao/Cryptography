using System;
using System.Collections.Generic;
using Crypto1.Enums;
using Crypto1.Interfaces;

namespace Crypto1
{
    public class SymmetricCypherAlgorithm
    {
        private const Int32 BlockSize = 8;
        private readonly EncryptionMode _encryptionMode;
        private readonly Byte[] _initializationVector;
        private ICypherAlgorithm Algorithm { get; set; }

        public SymmetricCypherAlgorithm(EncryptionMode encryptionMode, Byte[] initializationVector, ICypherAlgorithm algorithm)
        {
            _encryptionMode = encryptionMode;
            _initializationVector = initializationVector;
            Algorithm = algorithm;
        }
        
        public SymmetricCypherAlgorithm(Byte[] key, EncryptionMode mode, Byte[] initializationVector = null,
            params object[] list)
        {
            
        }

        public void Encrypt(Byte[] inputBlock, ref Byte[] encryptBlock)
        {
            
        }

        public void Decrypt(Byte[] inputBlock, ref Byte[] decryptBlock)
        {
            
        }
        
        public void Encrypt(String inputFile, String outputFile)
        {
            
        }

        public void Decrypt(String inputFile, String outputFile)
        {
            
        }
        
        public Byte[] Encrypt(Byte[] inputBlock)
        {
            Byte[] result = PadBuffer(inputBlock);
            List<Byte[]> blocks = new List<Byte[]>();

            switch (_encryptionMode)
            {
                case EncryptionMode.ECB:
                {
                    Byte[] block = new Byte[BlockSize];
                    for (var count = 0; count < result.Length / BlockSize; count++)
                    {
                        Array.Copy(result, count * BlockSize, block, 0, BlockSize);
                        blocks.Add(Algorithm.Encrypt(block));
                    }

                    break;
                }
                case EncryptionMode.CBC:
                {
                    var previousBlock = new Byte[BlockSize];
                    var currentBlock = new Byte[BlockSize];
                    Array.Copy(_initializationVector, previousBlock, previousBlock.Length);
                    for (var count = 0; count < result.Length / BlockSize; count++)
                    {
                        Array.Copy(result, count * BlockSize, currentBlock, 0, BlockSize);
                        blocks.Add(Algorithm.Encrypt(Xor(currentBlock, previousBlock)));
                        Array.Copy(blocks[count], previousBlock, BlockSize);
                    }
                    break;
                }
                case EncryptionMode.CFB:
                {
                    var previousBlock = new Byte[BlockSize];
                    var currentBlock = new Byte[BlockSize];
                    Array.Copy(_initializationVector, previousBlock, previousBlock.Length);
                    for (var count = 0; count < result.Length / BlockSize; count++)
                    {
                        Array.Copy(result, count * BlockSize, currentBlock, 0, BlockSize);
                        blocks.Add(Xor(Algorithm.Encrypt(previousBlock), currentBlock));
                        Array.Copy(blocks[count], previousBlock, BlockSize);
                    }
                    break;
                }
                case EncryptionMode.OFB:
                {
                    var previousBlock = new Byte[BlockSize];
                    var currentBlock = new Byte[BlockSize];
                    Array.Copy(_initializationVector, previousBlock, previousBlock.Length);
                    for (var count = 0; count < result.Length / BlockSize; count++)
                    {
                        Array.Copy(result, count * BlockSize, currentBlock, 0, BlockSize);
                        var encryptBlock = Algorithm.Encrypt(previousBlock);
                        blocks.Add(Xor(encryptBlock, currentBlock));
                        Array.Copy(encryptBlock, previousBlock, BlockSize);
                    }
                    break;
                }
                case EncryptionMode.CTR:
                {
                    var copyInitializationVector = new Byte[BlockSize];
                    _initializationVector.CopyTo(copyInitializationVector, 0);
                    var counter = BitConverter.ToUInt64(copyInitializationVector, 0);
                    var currentBlock = new Byte[BlockSize];
                    for (var count = 0; count < result.Length / BlockSize; count++)
                    {
                        Array.Copy(result, count * BlockSize, currentBlock, 0, BlockSize);
                        blocks.Add(Xor(Algorithm.Encrypt(copyInitializationVector), currentBlock));
                        counter++;
                        copyInitializationVector = BitConverter.GetBytes(counter);
                    }
                    break;
                }
                case EncryptionMode.RD:
                    break;
                case EncryptionMode.RD_H:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return ConvertListToArray(blocks);
        }

        public Byte[] Decrypt(Byte[] inputBlock)
        {
            var blocks = new List<Byte[]>();
            switch (_encryptionMode)
            {
                case EncryptionMode.ECB:
                {
                    var block = new Byte[BlockSize];
                    for (var count = 0; count < inputBlock.Length / BlockSize; count++)
                    {
                        Array.Copy(inputBlock, count * BlockSize, block, 0, BlockSize);
                        blocks.Add(Algorithm.Decrypt(block));
                    }
                    break;
                }
                case EncryptionMode.CBC:
                {
                    var previousBlock = new Byte[BlockSize];
                    var currentBlock = new Byte[BlockSize];
                    Array.Copy(_initializationVector, previousBlock, previousBlock.Length);
                    for (var i = 0; i < inputBlock.Length / BlockSize; i++)
                    {
                        Array.Copy(inputBlock, i * BlockSize, currentBlock, 0, BlockSize);
                        blocks.Add(Xor(previousBlock, Algorithm.Decrypt(currentBlock)));
                        Array.Copy(currentBlock, previousBlock, BlockSize);
                    }
                    break;
                }
                case EncryptionMode.CFB:
                {
                    var previousBlock = new Byte[BlockSize];
                    var currentBlock = new Byte[BlockSize];
                    Array.Copy(_initializationVector, previousBlock, previousBlock.Length);
                    for (var i = 0; i < inputBlock.Length / BlockSize; i++)
                    {
                        Array.Copy(inputBlock, i * BlockSize, currentBlock, 0, BlockSize);
                        blocks.Add(Xor(Algorithm.Encrypt(previousBlock), currentBlock));
                        Array.Copy(currentBlock, previousBlock, BlockSize);
                    }
                    break;
                }
                case EncryptionMode.OFB:
                {
                    var previousBlock = new Byte[BlockSize];
                    var currentBlock = new Byte[BlockSize];
                    Array.Copy(_initializationVector, previousBlock, previousBlock.Length);
                    for (var count = 0; count < inputBlock.Length / BlockSize; count++)
                    {
                        Array.Copy(inputBlock, count * BlockSize, currentBlock, 0, BlockSize);
                        var encryptBlock = Algorithm.Encrypt(previousBlock);
                        blocks.Add(Xor(encryptBlock, currentBlock));
                        Array.Copy(encryptBlock, previousBlock, BlockSize);
                    }
                    break;
                }
                case EncryptionMode.CTR:
                {
                    var copyInitializationVector = new Byte[BlockSize];
                    _initializationVector.CopyTo(copyInitializationVector, 0);
                    var counter = BitConverter.ToUInt64(copyInitializationVector, 0);
                    var currentBlock = new Byte[BlockSize];
                    for (var count = 0; count < inputBlock.Length / BlockSize; count++)
                    {
                        Array.Copy(inputBlock, count * BlockSize, currentBlock, 0, BlockSize);
                        blocks.Add(Xor(Algorithm.Encrypt(copyInitializationVector), currentBlock));
                        counter++;
                        copyInitializationVector = BitConverter.GetBytes(counter);
                    }
                    break;
                }
                case EncryptionMode.RD:
                    break;
                case EncryptionMode.RD_H:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var array = ConvertListToArray(blocks);
            var extraBlocks = array[array.Length - 1];
            var result = new Byte[array.Length - extraBlocks];
            Array.Copy(array, result, result.Length);
            return result;
        }
        
        private static Byte[] PadBuffer(Byte[] buf, Int32 padFrom, Int32 padTo, Padding padding = Padding.PKCS7) 
        {
            if ((padTo < buf.Length) | (padTo - padFrom > 255))
            {
                return buf;
            }
            var b = new Byte[padTo];
            Buffer.BlockCopy(buf, 0, b, 0, padFrom);
            
            for (var count = padFrom; count < padTo; count++) 
            {
                switch(padding) 
                {
                    case Padding.PKCS7:
                        b[count] = (Byte) (padTo - padFrom);
                        break;
                    case Padding.NONE:
                        b[count] = 0;
                        break;
                    default:
                        return buf;
                }
            }
            return b;
        }

        private static Byte[] PadBuffer(Byte[] buf, Padding padding = Padding.PKCS7) {
            var extraBlock = (buf.Length % BlockSize == 0) && padding == Padding.NONE ? 0 : 1;
            return PadBuffer(buf, buf.Length, ((buf.Length / BlockSize) + extraBlock) * BlockSize, padding);
        }
        
        private static Byte[] Xor(Byte[] left, Byte[] right)
        {
            var result = new Byte[left.Length];
            for (var count = 0; count < left.Length; count++)
            {
                result[count] = (Byte)(left[count] ^ right[count]);
            }
            return result;
        }

        private static Byte[] ConvertListToArray(List<Byte[]> data)
        {
            var array = new Byte[BlockSize * data.Count];
            for (var count = 0; count < data.Count; count++)
            {
                Array.Copy(data[count], 0, array, count * BlockSize, BlockSize);
            }
            return array;
        }
    }
}