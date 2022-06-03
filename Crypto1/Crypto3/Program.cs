using System;

namespace Crypto3
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            Byte[] key = new Byte[24];
            rnd.NextBytes(key);
            Byte[] text = new Byte[16];
            rnd.NextBytes(text);
            Byte[] iv = new Byte[16];
            rnd.NextBytes(iv);

            Rijndael rijndael = new Rijndael(4, 6, 0b100011011, new RoundKeysGenerator());
            Modes encryptor1 = new Modes(Modes.EncryptionMode.ECB, iv, Padder.PaddingType.PKCS7);
            encryptor1.algorithm = rijndael;
            rijndael.SetKey(key);

            byte[] encryptedText1 = encryptor1.EncryptBlock(text);
            byte[] decryptedText1 = encryptor1.DecryptBlock(encryptedText1);

        }
    }
}
