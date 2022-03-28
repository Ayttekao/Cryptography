using System;

namespace Crypto1.Interfaces
{
    public interface ICypherAlgorithm
    {
        Byte[] Encrypt(Byte[] inputBlock);
        Byte[] Decrypt(Byte[] inputBlock);
    }
}