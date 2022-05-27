using System;
using CourseWork.LOKI97.Algorithm.EncryptionTransformation;

namespace CourseWork.LOKI97.Algorithm.KeyGen
{
    public interface IKeyGen
    {
        public Object MakeKey(Byte[] k, IEncryptionTransformation encryptionTransformation);
    }
}