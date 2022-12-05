using System.Linq;

namespace CourseWork.SymmetricAlgorithms.CipherAlgorithm.E2.Algorithm;

public class E2Impl : ICipherAlgorithm
{
    private readonly byte[] _key;
    private const int BlockSize = 16;
    private const int KeyBytes = 16;
    private const int NumOfKeys = 16;

    public E2Impl(byte[] key)
    {
        _key = key;
    }

    public byte[] BlockEncrypt(byte[] input, int inOffset)
    {
        var roundKeys = GenerateRoundKeys(_key);
        return ITFaistelFT(input, roundKeys);
    }

    public byte[] BlockDecrypt(byte[] input, int inOffset)
    {
        var roundKeys = GenerateRoundKeys(_key);
        var roundKeysDecr = new byte[16][];

        for (var i = 0; i < 16; i++)
        {
            roundKeysDecr[i] = new byte[16];
        }

        for (var i = 0; i < 12; ++i)
        {
            for (var j = 0; j < KeyBytes; ++j)
            {
                roundKeysDecr[i][j] = roundKeys[11 - i][j];
            }
        }

        for (var i = 12; i < 16; ++i)
        {
            for (var j = 0; j < KeyBytes; ++j)
            {
                roundKeysDecr[i][j] = roundKeys[15 - (i - 12)][j];
            }
        }

        for (var i = 0; i < NumOfKeys; ++i)
        {
            for (var j = 0; j < KeyBytes; ++j)
            {
                roundKeys[i][j] = roundKeysDecr[i][j];
            }
        }

        return ITFaistelFT(input, roundKeysDecr);
    }

    public int GetBlockSize()
    {
        return BlockSize;
    }

    private byte[] ITFaistelFT(byte[] block, byte[][] roundKeys)
    {
        var M = IT(block, roundKeys[12], roundKeys[13]);
        var L = Functions.BytesToULong(M.Take(8).ToArray());
        var R = Functions.BytesToULong(M.Skip(8).Take(8).ToArray());
        var currentKey = new ulong[2];

        for (var i = 0; i < 11; ++i)
        {
            currentKey[0] = Functions.BytesToULong(roundKeys[i].Take(8).ToArray());
            currentKey[1] = Functions.BytesToULong(roundKeys[i].Skip(8).Take(8).ToArray());
            L ^= Functions.F(R, currentKey);
            (L, R) = (R, L);
        }

        currentKey[0] = Functions.BytesToULong(roundKeys[11].Take(8).ToArray());
        currentKey[1] = Functions.BytesToULong(roundKeys[11].Skip(8).Take(8).ToArray());
        L ^= Functions.F(R, currentKey);

        M = Functions.ULongToBytes(L).Concat(Functions.ULongToBytes(R)).ToArray();

        return FT(M, roundKeys[14], roundKeys[15]);
    }

    private byte[] IT(byte[] x, byte[] a, byte[] b)
    {
        var tempM = new byte[BlockSize];

        for (var i = 0; i < KeyBytes; i++)
        {
            tempM[i] = (byte)(x[i] ^ a[i]);
        }

        tempM = Functions.BinarX(tempM, b);

        return Functions.BP(tempM);
    }

    private byte[] FT(byte[] x, byte[] a, byte[] b)
    {
        var tempM = Functions.BPInv(x);

        tempM = Functions.BinarDeX(tempM, a);

        for (var i = 0; i < BlockSize; i++)
            tempM[i] ^= b[i];

        return tempM;
    }

    private byte[][] GenerateRoundKeys(byte[] key)
    {
        var roundKeys = new byte[NumOfKeys][];
        for (var i = 0; i < NumOfKeys; i++)
        {
            roundKeys[i] = new byte[KeyBytes];
        }

        const ulong g = 0x0123456789abcdef;
        var K = new ulong[4];

        K[0] = Functions.BytesToULong(key.Take(8).ToArray());
        K[1] = Functions.BytesToULong(key.Skip(4).Take(8).ToArray());

        switch (key.Length)
        {
            case 16:
                K[2] = Functions.S(Functions.S(Functions.S(g)));
                K[3] = Functions.S(K[2]);
                break;
            case 24:
                K[2] = Functions.BytesToULong(key.Skip(8).Take(8).ToArray());
                K[3] = Functions.S(Functions.S(Functions.S(Functions.S(g))));
                break;
            case 32:
                K[2] = Functions.BytesToULong(key.Skip(8).Take(8).ToArray());
                K[3] = Functions.BytesToULong(key.Skip(12).Take(8).ToArray());
                break;
        }

        var L = new ulong[] { 0, 0, 0, 0 };
        var Y = new ulong[] { 0, 0, 0, 0 };
        var U = g;
        ulong new_U;

        Functions.G(K, U, ref L, ref Y, out new_U);

        U = new_U;

        var q = new byte[32][];
        for (var i = 0; i < 32; i++)
            q[i] = new byte[8];

        for (var i = 0; i < 8; i++)
        {
            var YNew = new ulong[] { 0, 0, 0, 0 };

            Functions.G(Y, U, ref L, ref YNew, out new_U);

            for (var j = 0; j < 4; j++)
                Y[j] = YNew[j];

            U = new_U;

            for (var j = 0; j < 4; j++)
                q[4 * i + j] = Functions.ULongToBytes(L[j]);
        }

        var p = 0;
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 16; j++)
            {
                roundKeys[2 * i + 1][j] = q[2 * j][p];
                roundKeys[2 * i][j] = q[2 * j + 1][p];
            }

            p++;
        }

        return roundKeys;
    }
}