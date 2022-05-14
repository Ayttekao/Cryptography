using System;
using System.Collections.Generic;
using System.Linq;
using Crypto1.CipherAlgorithm;
using Crypto1.CipherModes;
using Crypto1.EncryptionTransformation;
using Crypto1.RoundKey;

namespace Crypto1.Tests
{
    public class DESTest
    {
        public static List<String> Run()
        {
            var cipherModes = new List<CipherMode>();
            cipherModes.Add(CipherMode.ECB);
            cipherModes.Add(CipherMode.CBC);
            cipherModes.Add(CipherMode.CFB);
            cipherModes.Add(CipherMode.OFB);
            cipherModes.Add(CipherMode.CTR);
            cipherModes.Add(CipherMode.RD);
            cipherModes.Add(CipherMode.RDH);
            
            var key = 11422891502611697239;
            var des = new DES(new RoundKeysGenerator(), new Encryption());
            des.SetKey(BitConverter.GetBytes(key));
            const string forHash = "Братья, по-любому... Спасибо вам, я... я вас никогда не забуду. " +
                                   "Клянусь, что никогда никого из вас я не оставлю в беде, Клянусь всем, что у меня " +
                                   "осталось. Клянусь, что никогда не пожалею о том, в чем сейчас клянусь. И никогда " +
                                   "не откажусь от своих слов. Клянусь.";
            var initVectorCTR = new Byte[] {0, 0, 0, 0, 0, 0, 0, 235};
            var initVectorRDOrRDH = new Byte[] {1, 0, 0, 0, 4, 0, 0, 8, 1, 0, 15, 0, 4, 10, 0, 2};
            var initVectorOther = new Byte[] { 2, 3, 3, 4, 5, 6, 7, 8 };
            var text = new Byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var cipherAlgorithms = new List<SymmetricCipherAlgorithm>();
            var results = new List<String>();
            foreach (var mode in cipherModes)
            {
                if (mode == CipherMode.CTR)
                {
                    cipherAlgorithms.Add(new SymmetricCipherAlgorithm(mode, initVectorCTR, des, forHash));

                }
                else if (mode is CipherMode.RD or CipherMode.RDH)
                {
                    cipherAlgorithms.Add(new SymmetricCipherAlgorithm(mode, initVectorRDOrRDH, des, forHash));
                }
                else
                {
                    cipherAlgorithms.Add(new SymmetricCipherAlgorithm(mode, initVectorOther, des, forHash));
                }
                
                results.Add(ResultTest(cipherAlgorithms.Last(), mode, text));
            }

            return results;
        }

        private static String ResultTest(SymmetricCipherAlgorithm cipher, CipherMode cipherMode, Byte[] inputBlock)
        {
            try
            {
                var encryptedText = cipher.Encrypt(inputBlock);
                var decryptedText = cipher.Decrypt(encryptedText);
                var compare = inputBlock.SequenceEqual(decryptedText) ? "PASSED" : "FAIL";
                return $"DES checker [RESULT]: {compare} [MODE]: {cipherMode}";
            }
            catch
            {
                return $"DES checker [RESULT]: FAIL [MODE]: {cipherMode}";
            }
        }
    }
}