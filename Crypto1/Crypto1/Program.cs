using System;
using System.Collections.Generic;
using System.Text;
using Crypto1.CipherModes;
using Crypto1.CypherAlgorithm;
using Crypto1.EncryptionTransformation;
using Crypto1.RoundKey;
using static Crypto1.Stuff.Utils;

namespace Crypto1
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            /*var result = Permutation(Convert.ToUInt64("1100", 2), new Byte[]{2, 4, 3, 1});
            var binary = Convert.ToString((long)result, 2);
            Console.WriteLine(binary);

            var dictionary = new Dictionary<Byte, Byte>()
            {
                {Convert.ToByte("11", 2), Convert.ToByte("00", 2)},
                {Convert.ToByte("10", 2), Convert.ToByte("01", 2)},
                {Convert.ToByte("01", 2), Convert.ToByte("11", 2)},
                {Convert.ToByte("00", 2), Convert.ToByte("11", 2)}
            };
            var result2 = Replacement(Convert.ToUInt64("1110", 2), dictionary, 2);
            Console.WriteLine(Convert.ToString((long)result2, 2));*/
            
            var des = new DES(new RoundKeysGenerator(), new Encryption());
            string forHash =
                "Братья, по-любому... Спасибо вам, я... я вас никогда не забуду. " +
                "Клянусь, что никогда никого из вас я не оставлю в беде, Клянусь всем, что у меня осталось. " +
                "Клянусь, что никогда не пожалею о том, в чем сейчас клянусь. И никогда не откажусь от своих слов. " +
                "Клянусь.";
            //var initVector = new byte[8] {0, 0, 0, 0, 0, 0, 0, 235};
            var initVector = new byte[16] {1, 0, 0, 0, 4, 0, 0, 8, 1, 0, 15, 0, 4, 10, 0, 2};
            //var initVector = new byte[8] { 2, 3, 3, 4, 5, 6, 7, 8 };
            var encryptor = new SymmetricCypherAlgorithm(CipherMode.RDH, initVector, des, forHash);
            var key = 11422891502611697239;
            var res = BitConverter.GetBytes(key);
            var text = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            des.SetKey(BitConverter.GetBytes(key));

            /*var encryptedText1 = encryptor.Encrypt(Encoding.Default.GetBytes(b));
            var decryptedText1 = Encoding.Default.GetString(encryptor.Decrypt(encryptedText1));*/
            var encryptedText = encryptor.Encrypt(text);
            var decryptedText = encryptor.Decrypt(encryptedText);
        }
    }
}