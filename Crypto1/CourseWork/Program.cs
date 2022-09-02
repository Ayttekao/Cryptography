using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.FileProcessing;
using CourseWork.SymmetricAlgorithms.AlgorithmService.Modes;
using CourseWork.SymmetricAlgorithms.TwoFish.Algorithm;

namespace CourseWork
{
    class Program
    {
        static Task Main(string[] args)
        {
            Byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15 };
    
            Byte[] initializationVector = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15};

            Byte[] input = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            const string filePath = @"C:\Users\Ayttekao\Downloads\4wAAAgCpBuA-1920.jpg";
            
            /*var twoFish = new TwoFish(key);

            var encryptBlock = twoFish.EncryptBlock(input);
            var decryptBlock = twoFish.DecryptBlock(encryptBlock);*/

            var aboba = File.ReadAllBytes(filePath);
            var twoFishImpl = new TwoFishImpl(key);
            var parallelCipher = new ParallelCipher(twoFishImpl, initializationVector);
            var encrypt = parallelCipher.Encrypt(filePath, EncryptionMode.ECB).Result;
            var decrypt = parallelCipher.Decrypt(encrypt, EncryptionMode.ECB).Result;
            
            Console.WriteLine(aboba.SequenceEqual(decrypt));
            
            return Task.CompletedTask;
        }
    }
}