using System;
using System.Collections.Generic;
using System.Numerics;
using CourseWork.AsymmetricAlgorithms.Benaloh.Algorithm;

namespace CipherStuffs.Handshake;

public interface IHandshaker
{
    public Byte[] GenerateSessionKey(Int32 keySize);
    public List<BigInteger> EncryptSessionKey(Byte[] sessionKey);
    public Byte[] DecryptSessionKey(List<BigInteger> encryptSessionKey);
    public PublicKey GetPublicKey();
}