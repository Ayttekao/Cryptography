using System;
using System.IO;
using System.Linq;
using System.Text;
using CourseWork.FileProcessing;
using CourseWork.LOKI97.Algorithm.BlockPacker;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;
using CourseWork.LOKI97.Algorithm.EncryptionTransformation;
using CourseWork.LOKI97.Algorithm.KeyGen;
using CourseWork.LOKI97.AlgorithmService;
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
            var encryptionMode = EncryptionMode.ECB;
            var algo = new AlgorithmService();
            
            Byte[] initializationVector = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15};
            
            Byte[] initializationVectorRD =
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15,
                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            Byte[] key =
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27,
                28, 29, 30, 31
            };

            var iv = initializationVector;

            var loki97 = new Loki97Impl(new Encryption(), new BlockPacker(), new KeyGen(), key);

            var parallelCipher = new ParallelCipher(loki97, iv, blockSize);
            
            var encryptedByteArray =
                algo.RunAlgorithm(aboba, key, iv, encryptionMode, true);

            /*
             * ECB 
             * CBC +
             * CFB +
             * CTR +
             * OFB +
             * RD +
             * RDH +
             */

            var encrypt = parallelCipher.Encrypt(filePath, encryptionMode);

            if (!encrypt.SequenceEqual(encryptedByteArray))
            {
                for (var index = 0; index < encrypt.Length; index++)
                {
                    if (encrypt[index] != encryptedByteArray[index])
                    {
                        Console.WriteLine("Wrong stream encrypt from index: {0}", index);
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Stream encrypt equals serial");
            }
            
            var decrypt = parallelCipher.Decrypt(encrypt, encryptionMode);

            var decryptedByteArray = 
                algo.RunAlgorithm(encryptedByteArray, key, iv, encryptionMode, false);
            
            Console.WriteLine("Serial decrypt and read file equals: {0}", decrypt.SequenceEqual(aboba));
            
            if (!decrypt.SequenceEqual(aboba))
            {
                for (var index = 0; index < decrypt.Length; index++)
                {
                    if (decrypt[index] != aboba[index])
                    {
                        Console.WriteLine("Wrong parallel decrypt from index: {0}", index);
                        break;
                    }
                }
            }
            
            //Console.WriteLine(temp.GetString(decrypt));
        }
    }
}