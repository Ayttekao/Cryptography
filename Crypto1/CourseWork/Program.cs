using System;
using System.Linq;
using System.Text;
using CourseWork.LOKI97.AlgorithmService;
using CourseWork.LOKI97.AlgorithmService.Modes;

namespace CourseWork
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25,
                26, 27, 28, 29, 30, 31};
    
            byte[] initializationVector = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15};
            
            byte[] initializationVectorRD =
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15,
                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            byte[] input = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25,
                26, 27, 28, 29, 30, 31, 32 };
            
            byte[] bytes = Encoding.Default.GetBytes("StupidStupidStupidStupidStupidStupidStupidStupidStupid");
            Console.WriteLine("Byte Array is: " + String.Join(" ", bytes));
            /*
             * CBC -- OK
             * CFB -- OK
             * CTR -- OK
             * ECB -- OK
             * OFB -- OK
             */


            var passCount = 0;
            var failCount = 0;
            for (var i = 0; i < 1000; i++)
            {
                try
                {
                    byte[] encryptedByteArray =
                        AlgorithmService.RunAlgorithm(bytes, key, initializationVector, EncryptionMode.CTR, true);
                    byte[] decryptedByteArray = AlgorithmService.RunAlgorithm(encryptedByteArray, key,
                        initializationVector, EncryptionMode.CTR, false);
                    if (bytes.SequenceEqual(decryptedByteArray))
                    {
                        passCount++;
                    }
                    else
                    {
                        failCount++;
                    }
                }
                catch
                {
                    failCount++;
                }
                
            }
            Console.WriteLine($"PASS: {passCount}\nFAIL: {failCount}");

            /*byte[] encryptedByteArray = AlgorithmService.RunAlgorithm(bytes, key, initializationVector, EncryptionMode.CBC, true);
            byte[] decryptedByteArray = AlgorithmService.RunAlgorithm(encryptedByteArray, key, initializationVector, EncryptionMode.CBC, false);
            string str = Encoding.Default.GetString(decryptedByteArray);
            Console.WriteLine("The String is: " + str);
            Console.WriteLine(bytes.SequenceEqual(decryptedByteArray));*/
        }
    }
}