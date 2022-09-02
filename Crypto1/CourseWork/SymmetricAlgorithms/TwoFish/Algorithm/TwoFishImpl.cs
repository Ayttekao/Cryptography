using System;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm;

namespace CourseWork.SymmetricAlgorithms.TwoFish.Algorithm;

public class TwoFishImpl : ICipherAlgorithm
{
    private uint[] _k;
    private byte[,] _s;
    private byte _start;

    public TwoFishImpl(byte[] key)
    {
        _s = new byte[2, 16];
        _k = new uint[40];
        var innerKey = new byte[32];
        int k, x, y;
        byte[] tmp = new byte[4], tmp2, M;
        uint A, B;

        Array.Copy(key, innerKey, key.Length);
        k = key.Length / 8;
        M = (byte[])innerKey.Clone();

        for (x = 0; x < k; x++)
        {
            Functions.RsMult(M, out var res, x);
            _s[0, 4 * x] = res[0];
            _s[0, 4 * x + 1] = res[1];
            _s[0, 4 * x + 2] = res[2];
            _s[0, 4 * x + 3] = res[3];
        }

        for (x = 0; x < 20; x++)
        {
            for (y = 0; y < 4; y++)
                tmp[y] = (byte)(x + x);

            Functions.h_func(tmp, out tmp2, ref M, k, 0);
            A = (uint)BitConverter.ToInt32(tmp2, 0) & 0xFFFFFFFF;

            for (y = 0; y < 4; y++)
                tmp[y] = (byte)(x + x + 1);

            Functions.h_func(tmp, out tmp2, ref M, k, 1);
            B = (uint)BitConverter.ToInt32(tmp2, 0) & 0xFFFFFFFF;
            B = Functions.ROL(B, 8);

            _k[x + x] = (A + B) & 0xFFFFFFFF;
            _k[x + x + 1] = Functions.ROL(B + B + A, 9);
        }

        _start = k switch
        {
            4 => 0,
            3 => 1,
            _ => 2
        };
    }

    public byte[] BlockEncrypt(byte[] input, int inOffset)
    {
        uint a, b, c, d, ta, tb, tc, td, t1, t2;
        int r;
        var result = new byte[16];
        var i = 8;
        var k = new uint[4];
        a = (uint)BitConverter.ToInt32(input, 0) & 0xFFFFFFFF;
        b = (uint)BitConverter.ToInt32(input, 4) & 0xFFFFFFFF;
        c = (uint)BitConverter.ToInt32(input, 8) & 0xFFFFFFFF;
        d = (uint)BitConverter.ToInt32(input, 12) & 0xFFFFFFFF;
        a ^= _k[0];
        b ^= _k[1];
        c ^= _k[2];
        d ^= _k[3];
        Array.Copy(_k, i, k, 0, 4);

        for (r = 8; r != 0; --r)
        {
            t2 = Functions.g_func(Functions.ROL(b, 8), _s, _start);
            t1 = Functions.g_func(a, _s, _start) + t2;
            c = Functions.ROR(c ^ (t1 + k[0]), 1);
            d = Functions.ROL(d, 1) ^ (t2 + t1 + k[1]);
            t2 = Functions.g_func(Functions.ROL(d, 8), _s, _start);
            t1 = Functions.g_func(c, _s, _start) + t2;
            a = Functions.ROR(a ^ (t1 + k[2]), 1);
            b = Functions.ROL(b, 1) ^ (t2 + t1 + k[3]);

            if (r == 1)
                break;

            i += 4;
            Array.Copy(_k, i, k, 0, 4);
        }

        ta = c ^ _k[4];
        tb = d ^ _k[5];
        tc = a ^ _k[6];
        td = b ^ _k[7];
        Array.Copy(BitConverter.GetBytes(ta), result, 4);
        Array.Copy(BitConverter.GetBytes(tb), 0, result, 4, 4);
        Array.Copy(BitConverter.GetBytes(tc), 0, result, 8, 4);
        Array.Copy(BitConverter.GetBytes(td), 0, result, 12, 4);

        return result;
    }

    public byte[] BlockDecrypt(byte[] input, int inOffset)
    {
        uint a, b, c, d, ta, tb, tc, td, t1, t2;
        byte[] result = new byte[16];
        ta = (uint)BitConverter.ToInt32(input, 0) & 0xFFFFFFFF;
        tb = (uint)BitConverter.ToInt32(input, 4) & 0xFFFFFFFF;
        tc = (uint)BitConverter.ToInt32(input, 8) & 0xFFFFFFFF;
        td = (uint)BitConverter.ToInt32(input, 12) & 0xFFFFFFFF;
        a = tc ^ _k[6];
        b = td ^ _k[7];
        c = ta ^ _k[4];
        d = tb ^ _k[5];

        int i = 36;
        uint[] k = new uint[4];
        Array.Copy(_k, i, k, 0, 4);

        for (int r = 8; r != 0; --r)
        {
            t2 = Functions.g_func(Functions.ROL(d, 8), _s, _start);
            t1 = Functions.g_func(c, _s, _start) + t2;
            a = Functions.ROL(a, 1) ^ (t1 + k[2]);
            b = Functions.ROR(b ^ (t2 + t1 + k[3]), 1);
            t2 = Functions.g_func(Functions.ROL(b, 8), _s, _start);
            t1 = Functions.g_func(a, _s, _start) + t2;
            c = Functions.ROL(c, 1) ^ (t1 + k[0]);
            d = Functions.ROR(d ^ (t2 + t1 + k[1]), 1);

            if (r == 1)
                break;

            i -= 4;
            Array.Copy(_k, i, k, 0, 4);
        }

        a ^= _k[0];
        b ^= _k[1];
        c ^= _k[2];
        d ^= _k[3];
        Array.Copy(BitConverter.GetBytes(a), result, 4);
        Array.Copy(BitConverter.GetBytes(b), 0, result, 4, 4);
        Array.Copy(BitConverter.GetBytes(c), 0, result, 8, 4);
        Array.Copy(BitConverter.GetBytes(d), 0, result, 12, 4);

        return result;
    }

    public int GetBlockSize()
    {
        return 16;
    }
}