using System;

namespace CourseWork.SymmetricAlgorithms.CipherAlgorithm.LOKI97.Algorithm.EncryptionTransformation
{
    public interface IEncryptionTransformation
    {
        UInt64 Compute(UInt64 A, UInt64 B);
    }
}