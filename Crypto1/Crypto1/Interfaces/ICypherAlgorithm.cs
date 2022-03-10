using System;

namespace Crypto1.Interfaces
{
    public interface ICypherAlgorithm
    {
        Byte[] Encrypt(Byte[] encryptBlock);
        Byte[] Decrypt(Byte[] decryptBlock);
    }
}