using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crypto1
{
    public static class Utils
    {
        /// <summary>
        /// Method of permuting bits within the passed value 
        /// </summary>
        /// <param name="src">Value for permutation</param>
        /// <param name="permutationRule">Permutation rule array</param>
        /// <returns>Permuted array</returns>
        /// <exception cref="ArgumentException">Occurs when the length of the value and the array are different</exception>
        /// <exception cref="InvalidDataException">Occurs when there are invalid indices in the permutation array</exception>
        public static UInt64 Permutation(UInt64 src, Byte[] permutationRule)
        {
            UInt64 result = 0;
            var srcWidth = (Int16)(Math.Log(src, 2.0) + 1);

            if (srcWidth != permutationRule.Length)
            {
                throw new ArgumentException("Permutation rule length and value length are different");
            }

            if (permutationRule.Max() != permutationRule.Length)
            {
                throw new InvalidDataException("Permutation rule contains invalid indexes");
            }
            
            for (var i = 0; i < permutationRule.Length; i++) 
            {
                var srcPos = srcWidth - permutationRule[i];
                result = (result << 1) | (src >> srcPos & 0x01);
            }
            return result;
        }

        /// <summary>
        /// Method for replacing a group of bits of size k 
        /// </summary>
        /// <param name="src">Value for permutation</param>
        /// <param name="permutationRule">Dictionary with bit replacement rule</param>
        /// <param name="groupSize">Bit group size</param>
        /// <returns>Replaced array</returns>
        public static UInt64 Replacement(UInt64 src, Dictionary<Byte, Byte> permutationRule, Byte groupSize)
        {
            UInt64 result = 0;
            var srcWidth = (Int16)(Math.Log(src, 2.0) + 1);

            for (var i = 0; i < srcWidth; i += groupSize)
            {
                var bitsToPermute = (Byte)((src >> i) & 0b11);
                result |= (UInt32)(permutationRule[bitsToPermute] << i);
            }
            
            return result;
        }
    }
}