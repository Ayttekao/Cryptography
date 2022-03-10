using System;

namespace Crypto1.Interfaces
{
    public interface IEncryptionTransformation
    {
        Byte[] EncryptionTransformation(Byte[] inputBlock, Byte[] roundKey);
    }
}