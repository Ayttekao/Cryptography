using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public sealed class RD : EncryptionModeBase
    {
        public override Byte[] Encrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            var blockSize = cipherAlgorithm.GetBlockSize();
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count + 1).ToList();
            var counterList = GetCounterList(iv, blocksList.Count, blockSize);
            outputBuffer[0] = cipherAlgorithm.BlockEncrypt(GetInitial(iv, blockSize).ToByteArray(), 0);

            Parallel.For(0, blocksList.Count, count =>
                    
                outputBuffer[count + 1] = cipherAlgorithm.BlockEncrypt(Xor(counterList[count], blocksList[count]), 0) 
            );
            
            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public override Byte[] Decrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var counterList = GetCounterList(iv, blocksList.Count, cipherAlgorithm.GetBlockSize());
                    
            Parallel.For(1, blocksList.Count, count =>
                    
                outputBuffer[count - 1] = Xor(cipherAlgorithm.BlockDecrypt(blocksList[count], 0), counterList[count - 1]) 
            );
            
            outputBuffer.RemoveAt(outputBuffer.Count - 1);
            return outputBuffer.SelectMany(x => x).ToArray();
        }

        private BigInteger GetDelta(Byte[] iv, Int32 blockSize)
        {
            var deltaArr = new Byte[blockSize];
            Array.Copy(iv, blockSize, deltaArr, 0, blockSize);
            return new BigInteger(deltaArr);
        }

        private BigInteger GetInitial(Byte[] iv, Int32 blockSize)
        {
            var initial = new Byte[blockSize];
            Array.Copy(iv, 0, initial, 0, blockSize);
            return new BigInteger(initial);
        }

        private List<Byte[]> GetCounterList(Byte[] iv, Int32 size, Int32 blockSize)
        {
            var delta = GetDelta(iv, blockSize);
            
            var copyInitializationVector = new Byte[blockSize];
            Array.Copy(iv, 0, copyInitializationVector, 0, blockSize);
            var initializationVector = GetInitial(iv, blockSize);
            
            var counterList = new List<Byte[]>();
            for (var count = 0; count < size; count++)
            {
                counterList.Add(initializationVector.ToByteArray());
                initializationVector += delta;
            }

            return counterList;
        }
    }
}