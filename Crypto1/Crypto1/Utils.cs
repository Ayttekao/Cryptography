using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crypto1
{
    /// <summary>
    /// Helper Methods
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Method of permuting bits within the passed value 
        /// </summary>
        /// <param name="src">Value for permutation</param>
        /// <param name="permutationRule">Permutation rule array</param>
        /// <returns>Permuted array</returns>
        /// <exception cref="ArgumentNullException">Occurs when a null array has been received</exception>
        /// <exception cref="ArgumentException">Occurs when the length of the value and the array are different</exception>
        /// <exception cref="InvalidDataException">Occurs when there are invalid indices in the permutation array</exception>
        public static UInt64 Permutation(UInt64 src, Byte[] permutationRule)
        {
            UInt64 result = 0;
            var srcWidth = (Int16)(Math.Log(src, 2.0) + 1);

            if (permutationRule == null)
            {
                throw new ArgumentNullException(nameof(permutationRule));
            }

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
        /// <exception cref="ArgumentNullException">Occurs when a null permutationRule has been received</exception>
        /// <exception cref="ArgumentException">Occurs when the number of groups in the value is not a multiple group size</exception>
        public static UInt64 Replacement(UInt64 src, Dictionary<Byte, Byte> permutationRule, Byte groupSize)
        {
            UInt64 result = 0;
            var srcWidth = (Int16)(Math.Log(src, 2.0) + 1);

            if (permutationRule == null)
            {
                throw new ArgumentNullException(nameof(permutationRule));
            }

            if (srcWidth % groupSize != 0)
            {
                throw new ArgumentException("The number of groups in the value is not a multiple group size");
            }

            for (var i = 0; i < srcWidth; i += groupSize)
            {
                var bitsToPermute = (Byte)((src >> i) & 0b11);

                if (permutationRule.TryGetValue(bitsToPermute, out var bitsPerPermute))
                {
                    result |= (UInt32)(bitsPerPermute << i);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(Convert.ToString((long)bitsToPermute, 2) +
                                                          " value not found in permutation rule");
                }
            }
            
            return result;
        }
    }
}