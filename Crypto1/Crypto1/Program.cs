using System;
using Crypto1.Tests;

namespace Crypto1
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var testResult = DESTest.Run();
            foreach (var result in testResult)
            {
                Console.WriteLine(result);
            }
        }
    }
}