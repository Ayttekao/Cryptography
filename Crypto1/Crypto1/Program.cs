using System;
using Crypto1.CipherModes;
using Crypto1.CypherAlgorithm;
using Crypto1.EncryptionTransformation;
using Crypto1.RoundKey;

namespace Crypto1
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var des = new DES(new RoundKeysGenerator(), new Encryption());
            const string forHash = "Братья, по-любому... Спасибо вам, я... я вас никогда не забуду. " +
                                   "Клянусь, что никогда никого из вас я не оставлю в беде, Клянусь всем, что у меня " +
                                   "осталось. Клянусь, что никогда не пожалею о том, в чем сейчас клянусь. И никогда " +
                                   "не откажусь от своих слов. Клянусь.";
            //var initVector = new Byte[] {0, 0, 0, 0, 0, 0, 0, 235}; // CTR
            //var initVector = new Byte[] {1, 0, 0, 0, 4, 0, 0, 8, 1, 0, 15, 0, 4, 10, 0, 2}; // RD or RD+H
            var initVector = new Byte[] { 2, 3, 3, 4, 5, 6, 7, 8 }; // Other
            var encryptor = new SymmetricCypherAlgorithm(CipherMode.OFB, initVector, des, forHash);
            var key = 11422891502611697239;
            var res = BitConverter.GetBytes(key);
            var text = new Byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            des.SetKey(BitConverter.GetBytes(key));

            /*var encryptedText1 = encryptor.Encrypt(Encoding.Default.GetBytes(b));
            var decryptedText1 = Encoding.Default.GetString(encryptor.Decrypt(encryptedText1));*/
            var encryptedText = encryptor.Encrypt(text);
            var decryptedText = encryptor.Decrypt(encryptedText);
        }
    }
}