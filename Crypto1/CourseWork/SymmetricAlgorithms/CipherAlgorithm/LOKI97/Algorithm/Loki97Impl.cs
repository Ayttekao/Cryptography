using System;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm.LOKI97.Algorithm.BlockPacker;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm.LOKI97.Algorithm.EncryptionTransformation;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm.LOKI97.Algorithm.KeyGen;

namespace CourseWork.SymmetricAlgorithms.CipherAlgorithm.LOKI97.Algorithm
{
    public sealed class Loki97Impl : ICipherAlgorithm
    {
        private const UInt32 Rounds = 16;
        private const UInt32 NumSubKeys = 48;
        private const Int32 BlockSize = 16;
        private IEncryptionTransformation _encryptionTransformation;
        private IBlockPacker _blockPacker;
        private Object _keys;

        public Loki97Impl(IEncryptionTransformation encryptionTransformation,
            IBlockPacker blockPacker,
            IKeyGen keyGen,
            Object key)
        {
            _blockPacker = blockPacker;
            _encryptionTransformation = encryptionTransformation;
            _keys = keyGen.MakeKey((Byte[])key, _encryptionTransformation);
        }
        
        public Byte[] BlockEncrypt(Byte[] input, int inOffset)
        {
            UInt64[] SK = (UInt64[]) _keys;

            var (L, R) = _blockPacker.PackBlock(input, inOffset);

            UInt32 k = 0;
            for (var i = 0; i < Rounds; i++)
            {
                var nR = R + SK[k++];
                var fOut = _encryptionTransformation.Compute(nR, SK[k++]);
                nR += SK[k++];
                R = L ^ fOut;
                L = nR;

            }

            return _blockPacker.UnpackBlock(L, R);
        }

        public Byte[] BlockDecrypt(Byte[] input, int inOffset)
        {
            UInt64[] SK = (UInt64[]) _keys;

            var (L, R) = _blockPacker.PackBlock(input, inOffset);

            UInt32 k = NumSubKeys - 1;
            for (var i = 0; i < Rounds; i++)
            {
                var nR = R - SK[k--];
                var fOut = _encryptionTransformation.Compute(nR, SK[k--]);
                nR -= SK[k--];
                R = L ^ fOut;
                L = nR;
            }

            return _blockPacker.UnpackBlock(L, R);
        }

        public int GetBlockSize()
        {
            return BlockSize;
        }
    }
}