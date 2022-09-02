using System;

namespace CourseWork.SymmetricAlgorithms.CipherAlgorithm
{
    public interface ICipherAlgorithm
    {
        Byte[] BlockEncrypt(Byte[] input, int inOffset);
        Byte[] BlockDecrypt(Byte[] input, int inOffset);
        Int32 GetBlockSize();
    }
}