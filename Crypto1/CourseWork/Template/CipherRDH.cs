using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CourseWork.Stuff;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;

namespace CourseWork.Template
{
    public sealed class CipherRDH : CipherRD
    {
        private Byte[] _valueForHash;
        
        public CipherRDH(ICipherAlgorithm cipherAlgorithm, Byte[] valueForHash) : base(cipherAlgorithm)
        {
            _valueForHash = valueForHash;
        }

        protected override List<Byte[]> ModifyOnFirstStageEncrypt(ref List<Byte[]> blocksList, ref Byte[] iv)
        {
            var outputBuffer = new List<Byte[]>();
            var initial = Utils.GetInitial(iv, _cipherAlgorithm.GetBlockSize());

            var encryptInitial = _cipherAlgorithm.BlockEncrypt(initial, 0);
            var hashAlgorithm = MD5.Create();
            var encryptedHash =
                _cipherAlgorithm.BlockEncrypt(Utils.Xor(initial, hashAlgorithm.ComputeHash(_valueForHash)), 0);
            var firstBlock =
                _cipherAlgorithm.BlockEncrypt(
                    Utils.Xor(
                        Utils.GetInitial(iv, _cipherAlgorithm.GetBlockSize()), 
                        blocksList.First()), 0);

            blocksList.Remove(blocksList.First());
            outputBuffer.Add(encryptInitial);
            outputBuffer.Add(encryptedHash);
            outputBuffer.Add(firstBlock);

            return outputBuffer;
        }

        protected override List<Byte[]> ModifyOnFirstStageDecrypt(ref List<Byte[]> blocksList, ref Byte[] iv)
        {
            if (Utils.IsWrongInit(_cipherAlgorithm, iv, _valueForHash, blocksList[1]))
            {
                throw new ArgumentException();
            }

            var outputBuffer = new List<Byte[]>();
            var firstBlock = Utils.Xor(Utils.GetInitial(iv, _cipherAlgorithm.GetBlockSize()), _cipherAlgorithm.BlockDecrypt(blocksList[2], 0));
            blocksList.RemoveRange(0, 3);
            outputBuffer.Add(firstBlock);

            return outputBuffer;
        }
    }
}