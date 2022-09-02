using System;

namespace CourseWork.SymmetricAlgorithms.TwoFish.Algorithm;

public static class Functions
{
    private const uint MdsPoly = 0x169;
    private const uint RsPoly = 0x14D;

    private static readonly byte[,] Rs = {
        { 0x01, 0xA4, 0x55, 0x87, 0x5A, 0x58, 0xDB, 0x9E },
        { 0xA4, 0x56, 0x82, 0xF3, 0X1E, 0XC6, 0X68, 0XE5 },
        { 0X02, 0XA1, 0XFC, 0XC1, 0X47, 0XAE, 0X3D, 0X19 },
        { 0XA4, 0X55, 0X87, 0X5A, 0X58, 0XDB, 0X9E, 0X03 }
    };
    
    private static readonly byte[,] Qord =
    {
        { 1, 1, 0, 0, 1 },
        { 0, 1, 1, 0, 0 },
        { 0, 0, 0, 1, 1 },
        { 1, 0, 1, 1, 0 }
    };
    
    public static uint ROL(uint x, int y)
    {
        return (x << y) | (x >> (32 - y));
    }

    public static uint ROR(uint x, int y)
    {
        return (((x & 0xFFFFFFFF) >> (int)((y) & 31)) | (x << (int)(32 - ((y) & 31)))) & 0xFFFFFFFF;
    }

    private static uint GfMult(uint a, uint b, uint p)
    {
        var B = new uint[2];
        var P = new uint[2];
        P[1] = p;
        B[1] = b;
        var result = P[0] = B[0] = 0;

        for (var i = 0; i < 7; i++)
        {
            result ^= B[a & 1];
            a >>= 1;
            B[1] = P[B[1] >> 7] ^ (B[1] << 1);
        }

        result ^= B[a & 1];

        return result;
    }

    private static uint MdsColumnMult(byte @in, int col)
    {
        uint x01, x5B, xEF;

        x01 = @in;
        x5B = GfMult(@in, 0x5B, MdsPoly);
        xEF = GfMult(@in, 0xEF, MdsPoly);

        return col switch
        {
            0 => (x01 << 0) | (x5B << 8) | (xEF << 16) | (xEF << 24),
            1 => (xEF << 0) | (xEF << 8) | (x5B << 16) | (x01 << 24),
            2 => (x5B << 0) | (xEF << 8) | (x01 << 16) | (xEF << 24),
            3 => (x5B << 0) | (x01 << 8) | (xEF << 16) | (x5B << 24),
            _ => 0
        };
    }

    private static void MdsMult(in byte[] @in, out byte[] @out)
    {
        int x;
        uint tmp = 0;

        for (x = 0; x < 4; x++)
        {
            tmp ^= MdsColumnMult(@in[x], x);
        }

        @out = BitConverter.GetBytes(tmp);
    }

    public static void RsMult(in byte[] @in, out byte[] @out, int len)
    {
        int x, y;
        @out = new byte[4];

        for (x = 0; x < 4; x++)
        {
            for (y = 0; y < 8; y++)
                @out[x] ^= (byte)GfMult(@in[y + len * 8], Rs[x, y], RsPoly);
        }
    }

    public static void h_func(in byte[] @in, out byte[] @out, ref byte[] m, int k, int offset)
    {
        var y = (byte[])@in.Clone();

        switch (k)
        {
            case 4:
                y[0] = (byte)(SBoxesGeneration.Sbox(1, y[0]) ^ m[4 * (6 + offset) + 0]);
                y[1] = (byte)(SBoxesGeneration.Sbox(0, y[1]) ^ m[4 * (6 + offset) + 1]);
                y[2] = (byte)(SBoxesGeneration.Sbox(0, y[2]) ^ m[4 * (6 + offset) + 2]);
                y[3] = (byte)(SBoxesGeneration.Sbox(1, y[3]) ^ m[4 * (6 + offset) + 3]);
                break;

            case 3:
                y[0] = (byte)(SBoxesGeneration.Sbox(1, y[0]) ^ m[4 * (4 + offset) + 0]);
                y[1] = (byte)(SBoxesGeneration.Sbox(1, y[1]) ^ m[4 * (4 + offset) + 1]);
                y[2] = (byte)(SBoxesGeneration.Sbox(0, y[2]) ^ m[4 * (4 + offset) + 2]);
                y[3] = (byte)(SBoxesGeneration.Sbox(0, y[3]) ^ m[4 * (4 + offset) + 3]);
                break;

            case 2:
                y[0] = (byte)(SBoxesGeneration.Sbox(1, SBoxesGeneration.Sbox(0, SBoxesGeneration.Sbox(0, y[0]) ^ m[4 * (2 + offset) + 0]) ^ m[4 * (0 + offset) + 0]));
                y[1] = (byte)(SBoxesGeneration.Sbox(0, SBoxesGeneration.Sbox(0, SBoxesGeneration.Sbox(1, y[1]) ^ m[4 * (2 + offset) + 1]) ^ m[4 * (0 + offset) + 1]));
                y[2] = (byte)(SBoxesGeneration.Sbox(1, SBoxesGeneration.Sbox(1, SBoxesGeneration.Sbox(0, y[2]) ^ m[4 * (2 + offset) + 2]) ^ m[4 * (0 + offset) + 2]));
                y[3] = (byte)(SBoxesGeneration.Sbox(0, SBoxesGeneration.Sbox(1, SBoxesGeneration.Sbox(1, y[3]) ^ m[4 * (2 + offset) + 3]) ^ m[4 * (0 + offset) + 3]));
                break;
        }

        MdsMult(in y, out @out);
    }

    public static uint g_func(uint x, byte[,] s, byte start)
    {
        byte g, i, y, z;
        uint res = 0;

        for (y = 0; y < 4; y++)
        {
            z = start;
            g = (byte)SBoxesGeneration.Sbox(Qord[y, z++], (x >> (8 * y)) & 255);
            i = 0;

            while (z != 5)
            {
                g = (byte)(g ^ s[0, (4 * i++) + y]);
                g = (byte)SBoxesGeneration.Sbox(Qord[y, z++], g);
            }

            res ^= MdsColumnMult(g, y);
        }

        return res;
    }
}