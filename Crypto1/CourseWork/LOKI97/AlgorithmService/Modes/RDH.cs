using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public sealed class RDH : EncryptionModeBase
    {
        private readonly Byte[] _valueForHash;
        public RDH(Byte[] valueForHash)
        {
            _valueForHash = valueForHash;
        }
        
        public override Byte[] Encrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            var hashAlgorithm = MD5.Create();
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count + 2).ToList();
            var counterList = GetCounterList(iv, blocksList.Count);
            var initial = GetInitialAsBiginteger(iv).ToByteArray();
            outputBuffer[0] = cipherAlgorithm.BlockEncrypt(initial, 0);
            outputBuffer[1] = Xor(initial, hashAlgorithm.ComputeHash(_valueForHash));
            
            Parallel.For(0, blocksList.Count, i =>
                    
                outputBuffer[i + 2] = cipherAlgorithm.BlockEncrypt(Xor(counterList[i], blocksList[i]), 0) 
            );
            
            return outputBuffer.SelectMany(x => x).ToArray();
        }

        public override Byte[] Decrypt(ICipherAlgorithm cipherAlgorithm, List<Byte[]> blocksList, Byte[] iv)
        {
            if (IsWrongInit(iv, _valueForHash, blocksList[1]))
            {
                throw new ArgumentException();
            }
            
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var counterList = GetCounterList(iv, blocksList.Count);

            Parallel.For(2, blocksList.Count, i =>
                    
                outputBuffer[i - 2] = Xor(cipherAlgorithm.BlockDecrypt(blocksList[i], 0), counterList[i - 2])
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