using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.FileProcessing;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm.E2.Algorithm;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm.Magenta.Algorithm;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm.TwoFish.Algorithm;
using CourseWork.SymmetricAlgorithms.Modes;

namespace CourseWork
{
    class Program
    {
        static Task Main(string[] args)
        {
            Byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15 };
    
            Byte[] initializationVector = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15};

            Byte[] input = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var path = @"D:\MyDownloads\images.png";
            var path2 = @"D:\MyDownloads\Decrypt.jpg";
            
            var file = File.ReadAllBytes(path);

            var magenta = new MagentaImpl(key);

            var e2 = new E2Impl(key);

            var twoFish = new TwoFishImpl(key);
            
            var cipher = new ParallelCipher(magenta, initializationVector);

            var encrypt = cipher.Encrypt(path, EncryptionMode.ECB).Result;

            var decrypt = cipher.Decrypt(encrypt, EncryptionMode.ECB).Result;
            
            Console.WriteLine(file.SequenceEqual(decrypt));
            
            File.WriteAllBytes(path2,decrypt);

            return Task.CompletedTask;
        }
    }
}