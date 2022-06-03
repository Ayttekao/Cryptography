using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWork.LOKI97.AlgorithmService;
using CourseWork.LOKI97.AlgorithmService.Modes;

namespace CourseWork
{
    public class Trash
    {
        public static async Task MainTrash()
        {
            /*var blockReader = new BlockReader(filePath, blockSize);
            Int32 iterations = blockReader.GetBlocksNumber() / processorCount;
            
            if (blockReader.GetBlocksNumber() % processorCount != 0)
            {
                iterations++;
            }

            for (var block = 0; block < iterations; block++)
            {
                listBytes.Add(blockReader.GetNextBlocks(processorCount).SelectMany(x => x).ToArray());
            }*/
            
            Byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25,
                26, 27, 28, 29, 30, 31};
    
            Byte[] initializationVector = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15};
            
            Byte[] initializationVectorRD =
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 69, 11, 12, 13, 14, 15,
                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            Byte[] input = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25,
                26, 27, 28, 29, 30, 31, 32 };
            
            /*Byte[] bytes = Encoding.Default.GetBytes(
                "In this world, is the destiny of mankind controlled by some transcendental entity or law? Is it " +
                "like the hand of God hovering above? At least, it is true that man has no control, even over his own will.");
            Console.WriteLine("Byte Array is: " + String.Join(" ", bytes));*/
            /*
             * RD -
             * RDH -
             * CBC -
             * CFB -
             * CTR +
             * ECB +
             * OFB -
             */
            
            var filePath =
                @"C:\Users\Ayttekao\Downloads\new 1.txt";
            
            var temp = new UTF8Encoding(true);
            var encryptionMode = EncryptionMode.CBC;
            var iv = initializationVector;
            var aboba = await File.ReadAllBytesAsync(filePath);
            var algo = new AlgorithmService();
            //var parallelRead = ParallelReadFile(filePath);
            
            //Console.WriteLine("Compare reads = {0}", aboba.SequenceEqual(parallelRead));
            //var amogus = GetDifferentIndexes(aboba, parallelRead);
            
            Byte[] encryptedByteArray =
                algo.RunAlgorithm(aboba, key, iv, encryptionMode, true);
            Byte[] decryptedByteArray = 
                algo.RunAlgorithm(encryptedByteArray, key, iv, encryptionMode, false);
            
            Console.WriteLine("Result decrypt from one thread = {0}", aboba.SequenceEqual(decryptedByteArray));
        }
        
        public static List<int> GetDifferentIndexes(Byte[] arr1, Byte[] arr2)
        {
            var lstDiffs = new List<int>();

            if (arr1?.Length == arr2?.Length)
            {  
                for (var idx = 0; idx < arr1?.Length; idx++)
                {
                    if (arr1[idx] != arr2[idx])
                    {
                        lstDiffs.Add(idx);
                    }
                }    
            }

            return lstDiffs;
        }
    }
}