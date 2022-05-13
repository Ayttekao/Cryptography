using System;
using Crypto1.Stuff;

namespace Crypto1.RoundKey
{
    public class RoundKeysGenerator : IRoundKeyGen
    {
        static Byte[] PC_1 =
        {
            50, 43, 36, 29, 22, 15,  8,  1, 51, 44, 37, 30, 23, 16,
            9,  2, 52, 45, 38, 31, 24, 17, 10,  3, 53, 46, 39, 32,
            56, 49, 42, 35, 28, 21, 14,  7, 55, 48, 41, 34, 27, 20,
            13,  6, 54, 47, 40, 33, 26, 19, 12,  5, 25, 18, 11,  4
        };
        
        static Byte[] PC_2 =
        {
            14, 17, 11, 24,  1,  5,  3, 28, 15,  6, 21, 10,
            23, 19, 12,  4, 26,  8, 16,  7, 27, 20, 13,  2,
            41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32
        };

        private static readonly Byte[] ShiftBits = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };

        public Byte[][] Generate(Byte[] key)
        {
            var roundKeys = new Byte[16][];
            var permutedKey = Utils.Permutation(PC_1, key);
            var res = BitConverter.ToUInt64(permutedKey, 0);
            var c = res >> 28;
            var d = res & ((1 << 28) - 1);
            for (var round = 0; round < 16; round++)
            {
                var shift = ShiftBits[round];
                c = ((c << shift) | (c >> (28 - shift))) & ((1 << 28) - 1);
                d = ((d << shift) | (d >> (28 - shift))) & ((1 << 28) - 1);

                roundKeys[round] = Utils.Permutation(PC_2, BitConverter.GetBytes((c << 28) | d));
            }
            return roundKeys;
        }
    }
}