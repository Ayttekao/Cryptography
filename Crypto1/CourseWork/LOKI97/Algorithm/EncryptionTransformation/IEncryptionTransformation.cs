using System;

namespace CourseWork.LOKI97.Algorithm.EncryptionTransformation
{
    public interface IEncryptionTransformation
    {
        UInt64 Compute(UInt64 A, UInt64 B);
    }
}