using System;
using System.Collections.Generic;
using static Crypto1.Utils;

namespace Crypto1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var result = Permutation(Convert.ToUInt64("1100", 2), new Byte[]{2, 4, 3, 1});
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
            Console.WriteLine(Convert.ToString((long)result2, 2));
        }
    }
}