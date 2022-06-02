using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CourseWork.Benaloh.Algorithm;
using CourseWork.Benaloh.ProbabilisticSimplicityTest;

namespace CipherStuffs.Handshake;

public class Handshaker : IHandshaker
{
    private BenalohImpl _benaloh;
    private BigInteger _ringModulo;

    public Handshaker(TestType testType, Double minProbability, UInt64 size, Int32 rValue, Int32 rExponent)
    {
        _ringModulo = BigInteger.Pow(rValue, rExponent) + BigInteger.One;
        _benaloh = new BenalohImpl(testType, minProbability, size, _ringModulo);
    }
    
    public Byte[] GenerateSessionKey(Int32 keySize)
    {
        var numberBytePairs = keySize / 2;
        var values = new Byte[numberBytePairs][];
        for (var i = 0; i < numberBytePairs; i++)
        {
            values[i] = Utils.RandomBigInteger(BigInteger.Zero, _ringModulo).ToByteArray();
        }

        return GetKeyByteArray(values, keySize);
    }

    public List<BigInteger> EncryptSessionKey(Byte[] sessionKey) // для этой пыжни нужны публичные ключи
    {
        return sessionKey.Select(t => _benaloh.Encrypt(t)).ToList();
    }

    public Byte[] DecryptSessionKey(List<BigInteger> encryptSessionKey) // а для этой приватные
    {
        var decryptSessionKey = new Byte[encryptSessionKey.Count][];

        for (var index = 0; index < encryptSessionKey.Count; index++)
        {
            decryptSessionKey[index] = _benaloh.Decrypt(encryptSessionKey[index]).ToByteArray();
        }

        return decryptSessionKey.Select(x => x.First()).ToArray();
    }

    public PublicKey GetPublicKey()
    {
        return _benaloh.GetPublicKey();
    }
    
    public void SetPublicKey(PublicKey publicKey)
    {
        _benaloh.SetPublicKey(publicKey);
    }
    
    private Byte[] GetKeyByteArray(Byte[][] keyByteArray, Int32 size)
    {
        var key = new byte[size];
        var count = 0;
        for (var i = 0; i < size / 2; i++)
        {
            for (var j = 0; j < size / 2 / 8; j++)
            {
                if (keyByteArray[i].Length > 1)
                {
                    key[count] = keyByteArray[i][j];
                }
                else
                {
                    key[count] = keyByteArray[i].First();
                    count++;
                    break;
                }
                count++;
            }
        }

        return key;
    }
}