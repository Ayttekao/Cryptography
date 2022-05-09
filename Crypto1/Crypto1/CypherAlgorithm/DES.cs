using System;
using Crypto1.EncryptionTransformation;
using Crypto1.RoundKey;
using static Crypto1.Stuff.Utils;

namespace Crypto1.CypherAlgorithm
{
    /*
     * 5 - Реализуйте алгоритм шифрования DES на базе класса из задания 4, определив свои реализации интерфейсов
     * 3.1 и 3.2. При реализации DES используйте функции, реализованные в заданиях 1 и 2.
     */
    public class DES : FeistelNetwork
    {
        private readonly Byte[] _initialPermutation = 
        {
            58,  50,  42,  34,  26,  18,  10,  2,  60,  52,  44,  36,  28,  20,  12,  4,
            62,  54,  46,  38,  30,  22,  14,  6,  64,  56,  48,  40,  32,  24,  16,  8,
            57,  49,  41,  33,  25,  17,  9,  1,  59,  51,  43,  35,  27,  19,  11,  3,
            61,  53,  45,  37,  29,  21,  13,  5,  63,  55,  47,  39,  31,  23,  15,  7
        };

        private readonly Byte[] _finalPermutation = 
        {
            40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9,  49, 17, 57, 25
        };

        public DES(IRoundKeyGen roundKeyGen, IEncryptionTransformation encryptionTransformation) : 
            base(encryptionTransformation, roundKeyGen)
        {

        }
        
        public override Byte[] Encrypt(Byte[] message) 
        {
            var firstPermutation = Permutation(_initialPermutation, message);
            var feistelTransformation = base.Encrypt(firstPermutation);
            var lastPermutation = Permutation(_finalPermutation, feistelTransformation);
            
            return lastPermutation;
        }

        public override Byte[] Decrypt(Byte[] message)
        {
            var firstPermutation = Permutation(_initialPermutation, message);
            var feistelTransformation = base.Decrypt(firstPermutation);
            var lastPermutation = Permutation(_finalPermutation, feistelTransformation);
            
            return lastPermutation;
        }
        
    }
}