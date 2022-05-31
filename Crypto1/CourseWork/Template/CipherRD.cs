using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;
using CourseWork.Stuff;

namespace CourseWork.Template
{
    public class CipherRD : CipherTemplate
    {
        private ICipherAlgorithm _cipherAlgorithm;

        public CipherRD(ICipherAlgorithm cipherAlgorithm)
        {
            _cipherAlgorithm = cipherAlgorithm;

        }
        
        protected override byte[] EncryptBlocks(List<byte[]> blocksList, ref byte[] iv)
        {
            var blockSize = _cipherAlgorithm.GetBlockSize();
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var counterList = GetCounterListV2(iv, blocksList.Count, blockSize);
            
            Array.Copy(sourceArray: counterList.Last(),
                sourceIndex: 0,
                destinationArray: iv,
                destinationIndex: 0,
                length: counterList.Last().Length);

            Parallel.For(0, blocksList.Count, count =>
                    
                outputBuffer[count] = _cipherAlgorithm.BlockEncrypt(Utils.Xor(counterList[count], blocksList[count]), 0) 
            );
            
            return outputBuffer.SelectMany(x => x).ToArray();
        }

        protected override byte[] DecryptBlocks(List<byte[]> blocksList, ref byte[] iv)
        {
            var outputBuffer = Enumerable.Repeat(default(Byte[]), blocksList.Count).ToList();
            var counterList = GetCounterListV2(iv, blocksList.Count, _cipherAlgorithm.GetBlockSize());
            
            Array.Copy(sourceArray: counterList.Last(),
                sourceIndex: 0,
                destinationArray: iv,
                destinationIndex: 0,
                length: counterList.Last().Length);

            Parallel.For(0, blocksList.Count, count =>
                    
                outputBuffer[count] = Utils.Xor(_cipherAlgorithm.BlockDecrypt(blocksList[count], 0), counterList[count]) 
            );
            
            return outputBuffer.SelectMany(x => x).ToArray();
        }

        protected override List<byte[]> ModifyOnFirstStageEncrypt(ref List<byte[]> blocksList, ref byte[] iv)
        {
            var outputBuffer = new List<Byte[]>();
            var initial =
                _cipherAlgorithm.BlockEncrypt(Utils.GetInitial(iv, _cipherAlgorithm.GetBlockSize()), 0);
            var firstBlock =
                _cipherAlgorithm.BlockEncrypt(
                    Utils.Xor(
                        Utils.GetInitial(iv, _cipherAlgorithm.GetBlockSize()), 
                        blocksList.First()), 0);
            blocksList.Remove(blocksList.First());
            
            outputBuffer.Add(initial);
            outputBuffer.Add(firstBlock);

            return outputBuffer;
        }

        protected override List<byte[]> ModifyOnFirstStageDecrypt(ref List<byte[]> blocksList, ref byte[] iv)
        {
            var outputBuffer = new List<Byte[]>();
            var initial =
                _cipherAlgorithm.BlockDecrypt(blocksList.First().ToArray(), 0);
            blocksList.Remove(blocksList.First());
            var firstBlock = Utils.Xor(initial, _cipherAlgorithm.BlockDecrypt(blocksList.First(), 0));
            blocksList.Remove(blocksList.First());
            
            outputBuffer.Add(firstBlock);

            return outputBuffer;
        }

        private static List<Byte[]> GetCounterListV2(Byte[] iv, Int32 size, Int32 blockSize)
        {
            var delta = Utils.GetDeltaAsBiginteger(iv, blockSize);
            var initializationVector = Utils.GetInitialAsBiginteger(iv, blockSize);
            var counterList = Enumerable.Repeat(default(Byte[]), size).ToList();
            
            for (var count = 0; count < counterList.Count; count++)
            {
                initializationVector += delta;
                counterList[count] = initializationVector.ToByteArray();
            }

            return counterList;
        }
    }
}