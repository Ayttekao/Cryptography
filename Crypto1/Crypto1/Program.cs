using System;
using Crypto1.Tests;

namespace Crypto1
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var failCount = 0;
            var passCount = 0;
            for (var i = 0; i < 10000; i++)
            {
                try
                {
                    var testResult = DESTest.Run();
                    passCount++;
                }
                catch
                {
                    failCount++;
                }
            }
            
            Console.WriteLine($"PASS: {passCount}\nFAIL: {failCount}");
            /*var testResult = DESTest.Run();
            foreach (var result in testResult)
            {
                Console.WriteLine(result);
            }*/
        }
    }
}