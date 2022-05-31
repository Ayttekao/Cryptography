using System;
using Crypto1.CipherAlgorithm;
using Crypto1.EncryptionTransformation;
using Crypto1.RoundKey;

namespace Crypto3
{
    /*
     * 2. На базе интерфейсов 3.1, 3.2, 3.3 из лабораторной работы 1 реализуйте класс, функционал которого позволяет
     * выполнять [де]шифрование блока данных алгоритмом AES при помощи вычисленных единожды из ключа шифрования,
     * раундовых ключей. Реализация алгоритма должен поддерживать работу с блоками длиной 128/192/256 бит и ключами
     * длиной 128/192/256 бит, а также предоставлять возможность настройки модуля над 𝐺𝐹(2^8)
     */
    public class AES : ICipherAlgorithm, IEncryptionTransformation, IRoundKeyGen
    {
        public Byte[] Encrypt(Byte[] inputBlock)
        {
            throw new System.NotImplementedException();
        }

        public Byte[] Decrypt(Byte[] inputBlock)
        {
            throw new System.NotImplementedException();
        }

        public Byte[] EncryptionTransformation(Byte[] inputBlock, Byte[] roundKey)
        {
            throw new System.NotImplementedException();
        }

        public Byte[][] Generate(Byte[] inputKey)
        {
            throw new System.NotImplementedException();
        }
    }
}