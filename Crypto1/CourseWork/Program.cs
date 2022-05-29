using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWork.FileProcessing;
using CourseWork.LOKI97.Algorithm.BlockPacker;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;
using CourseWork.LOKI97.Algorithm.EncryptionTransformation;
using CourseWork.LOKI97.Algorithm.KeyGen;
using CourseWork.LOKI97.AlgorithmService.Modes;

namespace CourseWork
{
    class Program
    {
        static void Main(string[] args)
        {
            var temp = new UTF8Encoding(true);
            var filePath = @"C:\Users\Ayttekao\Downloads\new 1.txt";
            var processorCount = Environment.ProcessorCount;
            var aboba = File.ReadAllBytes(filePath);
            var blockSize = 16;
            Byte[] key =
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27,
                28, 29, 30, 31
            };
            
            Byte[] initializationVector = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15};

            var loki97 = new Loki97Impl(new Encryption(), new BlockPacker(), new KeyGen(), key);

            var parallelCipher = new ParallelCipher(loki97, initializationVector, blockSize);

            var encrypt = parallelCipher.Encrypt(filePath, EncryptionMode.CBC);

            var decrypt = parallelCipher.Decrypt(encrypt, EncryptionMode.CBC);
            
            Console.WriteLine(decrypt.SequenceEqual(aboba));
        }
    }
}