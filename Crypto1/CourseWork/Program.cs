﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using CourseWork.Benalo.Algorithm;
using CourseWork.Benalo.ProbabilisticSimplicityTest;
using CourseWork.LOKI97.AlgorithmService;
using CourseWork.LOKI97.AlgorithmService.Modes;

namespace CourseWork
{
    class Program
    {
        static void Main(string[] args)
        {
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
            
            Byte[] bytes = Encoding.Default.GetBytes(
                "In this world, is the destiny of mankind controlled by some transcendental entity or law? Is it " +
                "like the hand of God hovering above? At least, it is true that man has no control, even over his own will.");
            Console.WriteLine("Byte Array is: " + String.Join(" ", bytes));

            /*
             * RD ++
             * RDH ++
             * CBC ++
             * CFB ++
             * CTR ++
             * ECB ++
             * OFB +
             */
            
            var aboba = File.ReadAllBytes(@"C:\Users\Ayttekao\Downloads\JoJo's Bizarre Adventure Opening 3 - 4K - 60FPS - Creditless.mp4");

            var encryptionMode = EncryptionMode.OFB;
            var iv = initializationVector;
            
            Byte[] encryptedByteArray =
                AlgorithmService.RunAlgorithm(aboba, key, iv, encryptionMode, true);
            Byte[] decryptedByteArray = 
                AlgorithmService.RunAlgorithm(encryptedByteArray, key, iv, encryptionMode, false);

            File.WriteAllBytes(@"C:\Users\Ayttekao\Desktop\JoJo's Bizarre Adventure Opening 3 - 4K - 60FPS - Creditless.mp4", decryptedByteArray);
            
            Console.WriteLine("Encrypted?: {0}", !aboba.SequenceEqual(encryptedByteArray));
            Console.WriteLine("Decrypted?: {0}", aboba.SequenceEqual(decryptedByteArray));
            
            /*var passCount = 0;
            var failCount = 0;
            for (var i = 0; i < 2; i++)
            {
                try
                {
                    Byte[] encryptedByteArray =
                        AlgorithmService.RunAlgorithm(bytes, key, initializationVectorRD, EncryptionMode.RD, true);
                    Byte[] decryptedByteArray = 
                        AlgorithmService.RunAlgorithm(encryptedByteArray, key, initializationVectorRD, EncryptionMode.RD, false);
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
            Console.WriteLine($"PASS: {passCount}\nFAIL: {failCount}");*/

            /*Byte[] encryptedByteArray =
                AlgorithmService.RunAlgorithm(bytes, key, initializationVectorRD, EncryptionMode.RD, true);
            Byte[] decryptedByteArray = 
                AlgorithmService.RunAlgorithm(encryptedByteArray, key, initializationVectorRD, EncryptionMode.RD, false);
            string str = Encoding.Default.GetString(decryptedByteArray);
            Console.WriteLine("The String is: " + str);
            Console.WriteLine(bytes.SequenceEqual(decryptedByteArray));*/

            /*var message = new BigInteger(25290);
            Console.WriteLine(message.GetByteCount());
            var benalo = new Benaloh(TestType.MillerRabin, 0.7, 30, message + 1);
            var encryptInt = benalo.BlockEncrypt(message);
            var decryptInt = benalo.BlockDecrypt(encryptInt);
            Console.WriteLine(message == decryptInt);*/
            
            /*var list = new List<BigInteger>();
            new PollardsRhoSolver().GetFactors(list, 13195);
            list.ForEach(s => Console.WriteLine(s));*/
        }
    }
}