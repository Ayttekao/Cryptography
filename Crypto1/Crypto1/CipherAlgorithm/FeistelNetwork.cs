using System;
using Crypto1.EncryptionTransformation;
using Crypto1.RoundKey;

namespace Crypto1.CipherAlgorithm
{
    /*
     * 4 На базе интерфейса 3.3 спроектируйте класс, реализующий функционал сети Фейстеля. Конструктор класса
     * должен принимать в качестве параметров реализации интерфейсов 3.1 и 3.2.
     */
    
    public class FeistelNetwork : ICipherAlgorithm
    {
        private IEncryptionTransformation _encryptionTransformation;
        private IRoundKeyGen _roundKeyGen;
        private Byte[][] _roundKeys;

        protected FeistelNetwork(IEncryptionTransformation encryptionTransformation, IRoundKeyGen roundKeyGen)
        {
            _encryptionTransformation = encryptionTransformation;
            _roundKeyGen = roundKeyGen;
        }
        
        public virtual Byte[] Encrypt(Byte[] inputBlock)
        {
            var res = BitConverter.ToUInt64(inputBlock, 0);
            var left = (UInt32)(res >> 32);
            var right = (UInt32)(res & ((UInt64)1 << 32) - 1);
            UInt32 newLeft = 0;
            UInt32 newRight = 0;
            
            for (var round = 0; round < 16; round++)
            {
                newLeft = right;
                newRight = left ^ BitConverter.ToUInt32(
                    startIndex: 0,
                    value: _encryptionTransformation
                        .EncryptionTransformation(BitConverter.GetBytes(right), _roundKeys[round]));
                left = newLeft;
                right = newRight;
            }
            res = newLeft;
            res = res << 32 | newRight;
            var result = BitConverter.GetBytes(res);
            
            return result;
        }

        public virtual Byte[] Decrypt(Byte[] inputBlock)
        {
            var res = BitConverter.ToUInt64(inputBlock, 0);
            var left = (UInt32)(res >> 32);
            var right = (UInt32)(res & ((UInt64)1 << 32) - 1);
            UInt32 newLeft = 0; 
            UInt32 newRight = 0;

            for (var round = 15; round >= 0; round--)
            {
                newRight = left;
                newLeft = right ^ BitConverter.ToUInt32(
                    startIndex: 0,
                    value: _encryptionTransformation
                        .EncryptionTransformation(BitConverter.GetBytes(left), _roundKeys[round]));
                left = newLeft;
                right = newRight;
            }
            res = newLeft;
            res = res << 32 | newRight;
            var result = BitConverter.GetBytes(res);
            
            return result;
        }
        
        public void SetKey(Byte[] key)
        {
            _roundKeys = _roundKeyGen.Generate(key);
        }
    }
}