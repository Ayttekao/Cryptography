using System.Linq;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;

namespace CourseWork.SymmetricAlgorithms.E2.Algorithm;

public class E2Impl : ICipherAlgorithm
{
    private int _blockSize = 16;
    private byte[] _key;
    private int _keyBytes = 16;
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
            roundKeysDecr[i] = new byte[16];

        for (var i = 0; i < 12; ++i)
        for (var j = 0; j < _keyBytes; ++j)
            roundKeysDecr[i][j] = roundKeys[11 - i][j];

        for (var i = 12; i < 16; ++i)
        for (var j = 0; j < _keyBytes; ++j)
            roundKeysDecr[i][j] = roundKeys[15 - (i - 12)][j];

        for (var i = 0; i < NumOfKeys; ++i)
        for (var j = 0; j < _keyBytes; ++j)
            roundKeys[i][j] = roundKeysDecr[i][j];

        return ITFaistelFT(input, roundKeysDecr);
    }

    public int GetBlockSize()
    {
        return _blockSize;
    }

    private byte[] ITFaistelFT(byte[] block, byte[][] roundKeys)
    {
        byte[] M = IT(block, roundKeys[12], roundKeys[13]);

        ulong L = Function.BytesToULong(M.Take(8).ToArray());
        ulong R = Function.BytesToULong(M.Skip(8).Take(8).ToArray());

        var currentKey = new ulong[2];
        for (var i = 0; i < 11; ++i)
        {
            currentKey[0] = Function.BytesToULong(roundKeys[i].Take(8).ToArray());
            currentKey[1] = Function.BytesToULong(roundKeys[i].Skip(8).Take(8).ToArray());
            L ^= Function.F(R, currentKey);
            (L, R) = (R, L);
        }

        currentKey[0] = Function.BytesToULong(roundKeys[11].Take(8).ToArray());
        currentKey[1] = Function.BytesToULong(roundKeys[11].Skip(8).Take(8).ToArray());
        L ^= Function.F(R, currentKey);

        M = Function.ULongToBytes(L).Concat(Function.ULongToBytes(R)).ToArray();

        return FT(M, roundKeys[14], roundKeys[15]);
    }

    private byte[] IT(byte[] x, byte[] a, byte[] b)
    {
        var tempM = new byte[_blockSize];
        for (var i = 0; i < _keyBytes; i++)
            tempM[i] = (byte)(x[i] ^ a[i]);
        tempM = Function.BinarX(tempM, b);

        return Function.BP(tempM);
    }

    private byte[] FT(byte[] x, byte[] a, byte[] b)
    {
        var tempM = Function.BPInv(x);

        tempM = Function.BinarDeX(tempM, a);

        for (var i = 0; i < _blockSize; i++)
            tempM[i] ^= b[i];

        return tempM;
    }

    //генерирует раундовые ключи
    private byte[][] GenerateRoundKeys(byte[] key)
    {
        var roundKeys = new byte[NumOfKeys][];
        for (int i = 0; i < NumOfKeys; i++)
            roundKeys[i] = new byte[_keyBytes];

        ulong g = 0x0123456789abcdef;
        ulong[] K = new ulong[4];

        K[0] = Function.BytesToULong(key.Take(8).ToArray());
        K[1] = Function.BytesToULong(key.Skip(4).Take(8).ToArray());

        if (key.Length == 16)
        {
            K[2] = Function.S(Function.S(Function.S(g)));
            K[3] = Function.S(K[2]);
        }
        else if (key.Length == 24)
        {
            K[2] = Function.BytesToULong(key.Skip(8).Take(8).ToArray());
            K[3] = Function.S(Function.S(Function.S(Function.S(g))));
        }
        else if (key.Length == 32)
        {
            K[2] = Function.BytesToULong(key.Skip(8).Take(8).ToArray());
            K[3] = Function.BytesToULong(key.Skip(12).Take(8).ToArray());
        }

        var L = new ulong[4] { 0, 0, 0, 0 };
        var Y = new ulong[4] { 0, 0, 0, 0 };
        var U = g;
        ulong new_U;

        Function.G(K, U, ref L, ref Y, out new_U);

        U = new_U;

        var q = new byte[32][];
        for (var i = 0; i < 32; i++)
            q[i] = new byte[8];

        for (var i = 0; i < 8; i++)
        {
            var Y_new = new ulong[4] { 0, 0, 0, 0 };

            Function.G(Y, U, ref L, ref Y_new, out new_U);

            for (var j = 0; j < 4; j++)
                Y[j] = Y_new[j];

            U = new_U;

            for (var j = 0; j < 4; j++)
                q[4 * i + j] = Function.ULongToBytes(L[j]);
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