using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        static async Task Main(string[] args)
        {
            var temp = new UTF8Encoding(true);
            var filePath = @"C:\Users\Ayttekao\Downloads\pardonUHD.jpg";
            var processorCount = Environment.ProcessorCount;
            var aboba = File.ReadAllBytes(filePath);
            var blockSize = 16;
            var encryptionMode = EncryptionMode.CBC;
            var algo = new AlgorithmService();

            Byte[] initializationVector = GetByteArray(16);

            Byte[] initializationVectorRD = GetByteArray(32);

            Byte[] key = GetByteArray(32);

            var iv = initializationVector;

            var loki97 = new Loki97Impl(new Encryption(), new BlockPacker(), new KeyGen(), key);

            var parallelCipher = new ParallelCipher(loki97, iv);
            
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

            var encrypt = await parallelCipher.Encrypt(filePath, encryptionMode);
            
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
            
            //var decrypt = await parallelCipher.Decrypt(encrypt, encryptionMode);
            // await parallelCipher.Decrypt(@"C:\Users\Ayttekao\Desktop\pardonUHD.jpg", encrypt, encryptionMode);

            var decryptedByteArray = 
                algo.RunAlgorithm(encryptedByteArray, key, iv, encryptionMode, false);
            
            //Console.WriteLine("Serial decrypt and read file equals: {0}", decrypt.SequenceEqual(aboba));
            
            /*if (!decrypt.SequenceEqual(aboba))
            {
                for (var index = 0; index < decrypt.Length; index++)
                {
                    if (decrypt[index] != aboba[index])
                    {
                        Console.WriteLine("Wrong parallel decrypt from index: {0}", index);
                        break;
                    }
                }
            }*/
            
            //Console.WriteLine(temp.GetString(decrypt));
        }
        
        private static Byte[] GetByteArray(int size)
        {
            var rnd = new Random();
            var b = new byte[size];
            rnd.NextBytes(b);
            return b;
        }
    }
}