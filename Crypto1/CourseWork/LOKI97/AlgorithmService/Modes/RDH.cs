using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Encoder = CourseWork.LOKI97.Algorithm.Encoder;
using Decoder = CourseWork.LOKI97.Algorithm.Decoder;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public sealed class RDH : EncryptionModeBase
    {
        public override Byte[] Encrypt(List<Byte[]> blocksList, object key, Byte[] iv)
        {
            var valueForHash = Encoding.Default.GetBytes("aboba");
            var hashAlgorithm = MD5.Create();
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count + 2).ToList();
            var encoder = new Encoder();
            var counterList = GetCounterList(iv, blocksList.Count);
            var initial = GetInitialAsBiginteger(iv).ToByteArray();
            outputBuffer[0] = encoder.BlockEncrypt(initial, 0, key);
            outputBuffer[1] = Xor(initial, hashAlgorithm.ComputeHash(valueForHash));
            
            Parallel.For(0, blocksList.Count, i =>
                    
                outputBuffer[i + 2] = encoder.BlockEncrypt(Xor(counterList[i], blocksList[i]), 0, key) 
            );
            
            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public override Byte[] Decrypt(List<Byte[]> blocksList, object key, Byte[] iv)
        {
            if (IsWrongInit(iv, Encoding.Default.GetBytes("aboba"), blocksList[1]))
            {
                throw new ArgumentException();
            }
            
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var decoder = new Decoder();
            var counterList = GetCounterList(iv, blocksList.Count);

            Parallel.For(2, blocksList.Count, i =>
                    
                outputBuffer[i - 2] = Xor(decoder.BlockDecrypt(blocksList[i], 0, key), counterList[i - 2])
            );
            outputBuffer.RemoveAt(outputBuffer.Count - 1);
            outputBuffer.RemoveAt(outputBuffer.Count - 1);
            
            return outputBuffer.SelectMany(x => x).ToArray();
        }

        private Boolean IsWrongInit(Byte[] iv, Byte[] valueForHash, Byte[] hashedValue)
        {
            var initial = GetInitial(iv);
            var hashAlgorithm = MD5.Create();

            return !Xor(initial, hashAlgorithm.ComputeHash(valueForHash)).SequenceEqual(hashedValue);
        }

        private BigInteger GetDeltaAsBiginteger(Byte[] iv)
        {
            return new BigInteger(GetDelta(iv));
        }

        private BigInteger GetInitialAsBiginteger(Byte[] iv)
        {
            return new BigInteger(GetInitial(iv));
        }

        private Byte[] GetDelta(Byte[] iv)
        {
            var deltaArr = new Byte[blockSize];
            Array.Copy(iv, blockSize, deltaArr, 0, blockSize);
            return deltaArr;
        }

        private Byte[] GetInitial(Byte[] iv)
        {
            var initial = new Byte[blockSize];
            Array.Copy(iv, 0, initial, 0, blockSize);
            return initial;
        }

        private List<Byte[]> GetCounterList(Byte[] iv, Int32 size)
        {
            var delta = GetDeltaAsBiginteger(iv);
            
            var copyInitializationVector = new Byte[blockSize];
            Array.Copy(iv, 0, copyInitializationVector, 0, blockSize);
            var initializationVector = GetInitialAsBiginteger(iv);
            
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