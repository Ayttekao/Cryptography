using System;

namespace Crypto1.Interfaces
{
    public interface IEncryptionTransformation
    {
        Byte[] Encrypt(Byte[] inputBlock, Byte[] roundKey);
    }
}