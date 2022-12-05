using System;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm.LOKI97.Algorithm.EncryptionTransformation;

namespace CourseWork.SymmetricAlgorithms.CipherAlgorithm.LOKI97.Algorithm.KeyGen
{
    public interface IKeyGen
    {
        public Object MakeKey(Byte[] k, IEncryptionTransformation encryptionTransformation);
    }
}