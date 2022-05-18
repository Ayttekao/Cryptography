using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public sealed class RD : EncryptionModeBase
    {
        public override Byte[] Encrypt(List<Byte[]> blocksList, object key, Byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count + 1).ToList();
            var encoder = new Encoder();
            var counterList = GetCounterList(iv, blocksList.Count);
            outputBuffer[0] = encoder.BlockEncrypt(GetInitial(iv).ToByteArray(), 0, key);

            Parallel.For(0, blocksList.Count, count =>
                    
                outputBuffer[count + 1] = encoder.BlockEncrypt(Xor(counterList[count], blocksList[count]), 0, key) 
            );
            
            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public override Byte[] Decrypt(List<Byte[]> blocksList, object key, Byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var decoder = new Decoder();
            var counterList = GetCounterList(iv, blocksList.Count);
                    
            Parallel.For(1, blocksList.Count, count =>
                    
                outputBuffer[count - 1] = Xor(decoder.BlockDecrypt(blocksList[count], 0, key), counterList[count - 1]) 
            );
            
            outputBuffer.RemoveAt(outputBuffer.Count - 1);
            return outputBuffer.SelectMany(x => x).ToArray();
        }

        private BigInteger GetDelta(Byte[] iv)
        {
            var deltaArr = new Byte[blockSize];
            Array.Copy(iv, blockSize, deltaArr, 0, blockSize);
            return new BigInteger(deltaArr);
        }

        private BigInteger GetInitial(Byte[] iv)
        {
            var initial = new Byte[blockSize];
            Array.Copy(iv, 0, initial, 0, blockSize);
            return new BigInteger(initial);
        }

        private List<Byte[]> GetCounterList(Byte[] iv, Int32 size)
        {
            var delta = GetDelta(iv);
            
            var copyInitializationVector = new Byte[blockSize];
            Array.Copy(iv, 0, copyInitializationVector, 0, blockSize);
            var initializationVector = GetInitial(iv);
            
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