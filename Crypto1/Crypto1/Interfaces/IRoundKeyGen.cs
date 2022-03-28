using System;

namespace Crypto1.Interfaces
{
    public interface IRoundKeyGen
    {
        Byte[,] Generate(Byte[] inputKey);
    }
}