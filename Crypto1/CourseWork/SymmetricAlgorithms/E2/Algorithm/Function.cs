using System;
using System.Linq;
using System.Numerics;

namespace CourseWork.SymmetricAlgorithms.E2.Algorithm;

public class Function
{
    private static byte[] SBox =
    {
        225, 66, 62, 129, 78, 23, 158, 253, 180, 63, 44, 218, 49, 30, 224, 65,
        204, 243, 130, 125, 124, 18, 142, 187, 228, 88, 21, 213, 111, 233, 76, 75,
        53, 123, 90, 154, 144, 69, 188, 248, 121, 214, 27, 136, 2, 171, 207, 100,
        9, 12, 240, 1, 164, 176, 246, 147, 67, 99, 134, 220, 17, 165, 131, 139,
        201, 208, 25, 149, 106, 161, 92, 36, 110, 80, 33, 128, 47, 231, 83, 15,
        145, 34, 4, 237, 166, 72, 73, 103, 236, 247, 192, 57, 206, 242, 45, 190,
        93, 28, 227, 135, 7, 13, 122, 244, 251, 50, 245, 140, 219, 143, 37, 150,
        168, 234, 205, 51, 101, 84, 6, 141, 137, 10, 94, 217, 22, 14, 113, 108,
        11, 255, 96, 210, 46, 211, 200, 85, 194, 35, 183, 116, 226, 155, 223, 119,
        43, 185, 60, 98, 19, 229, 148, 52, 177, 39, 132, 159, 215, 81, 0, 97,
        173, 133, 115, 3, 8, 64, 239, 104, 254, 151, 31, 222, 175, 102, 232, 184,
        174, 189, 179, 235, 198, 107, 71, 169, 216, 167, 114, 238, 29, 126, 170, 182,
        117, 203, 212, 48, 105, 32, 127, 55, 91, 157, 120, 163, 241, 118, 250, 5,
        61, 58, 68, 87, 59, 202, 199, 138, 24, 70, 156, 191, 186, 56, 86, 26,
        146, 77, 38, 41, 162, 152, 16, 153, 112, 160, 197, 40, 193, 109, 20, 172,
        249, 95, 79, 196, 195, 209, 252, 221, 178, 89, 230, 181, 54, 82, 74, 42
    };

    //функция возвращает 64 битное число типа ulong из массива из 8 элементов типа byte
    public static ulong BytesToULong(byte[] bytes)
    {
        ulong temp1 = 0;
        for (var i = 0; i < 8; ++i)
            temp1 = (temp1 << 8) | bytes[i];

        return temp1;
    }

    //функция записывает в d массив из 4 элементов типа byte на основе 32битного числа типа ulong
    private static byte[] UIntToBytes(uint number) => BitConverter.GetBytes(number).Reverse().ToArray();

    //функция возвращает 32 битное число из массива из 4 элементов типа byte
    private static uint BytesToUInt(byte[] bytes)
    {
        uint i32 = (uint)(bytes[3] | (bytes[2] << 8) | (bytes[1] << 16) | (bytes[0] << 24));

        return i32;
    }

    //функция записывает в res массив из 8 элементов типа byte на основе 64битного числа типа ulong
    public static byte[] ULongToBytes(ulong number)
    {
        return BitConverter.GetBytes(number).Reverse().ToArray();
    }

    //расширенный алгоритм Евклида
    private static ulong ExGCD(ulong a, ulong b, out ulong x, out ulong y)
    {
        if (b == 0)
        {
            x = 1;
            y = 0;
            return a;
        }

        ulong x1, y1;
        ulong d1 = ExGCD(b, a % b, out x1, out y1);
        x = y1;
        y = x1 - (a / b) * y1;

        return d1;
    }

    // функция для нахождения мультипликативного обратного к  a по модулю M на основе расширенного алгоритма Евклида
    private static ulong ReverseElement(ulong a, ulong m)
    {
        ulong x, y, d;
        d = ExGCD(a, m, out x, out y);

        if (d != 1)
            throw new Exception();
        else
            return x;
    }

    public static byte[] BinarX(byte[] x64, byte[] y64)
    {
        int num_of_bytes = 4;
        uint[] x = new uint[num_of_bytes];
        uint[] y = new uint[num_of_bytes];
        uint[] u = new uint[num_of_bytes];

        for (int i = 0; i < num_of_bytes; i++)
        {
            x[i] = BytesToUInt(x64.Skip(4 * i).Take(4).ToArray());
            y[i] = BytesToUInt(y64.Skip(4 * i).Take(4).ToArray());
        }

        for (int i = 0; i < num_of_bytes; i++)
        {
            uint z = y[i];
            z |= 1;
            u[i] = x[i] * z;
        }

        var u64 = new byte[] { };
        for (int i = 0; i < num_of_bytes; ++i)
            u64 = u64.Concat(UIntToBytes(u[i])).ToArray();

        return u64;
    }

    public static byte[] BinarDeX(byte[] x64, byte[] y64)
    {
        int numOfBytes = 4;
        var x = new uint[numOfBytes];
        var y = new uint[numOfBytes];
        var w = new uint[numOfBytes];

        for (var j = 0; j < numOfBytes; ++j)
        {
            x[j] = BytesToUInt(x64.Skip(4 * j).Take(4).ToArray());
            y[j] = BytesToUInt(y64.Skip(4 * j).Take(4).ToArray());
        }

        for (var i = 0; i < numOfBytes; ++i)
        {
            uint z = y[i];
            z |= 1;

            var pow2To32 = 1UL << 32;
            var x1 = ReverseElement(z, pow2To32);

            w[i] = (uint)(x[i] * x1);
        }

        var w64 = Array.Empty<byte>();
        for (var i = 0; i < numOfBytes; ++i)
            w64 = w64.Concat(UIntToBytes(w[i])).ToArray();

        return w64;
    }

    //BP(X)
    public static byte[] BP(byte[] x)
    {
        var result = new byte[16];
        for (int i = 0; i < 16; i++)
            result[i] = x[(5 * i) % 16];

        return result;
    }

    //BP^-1(X)
    public static byte[] BPInv(byte[] x)
    {
        var result = new byte[16];
        for (int i = 0; i < 16; i++)
            result[i] = x[(13 * i) % 16];

        return result;
    }

    //S(X) производится подстановка из таблицы замен
    public static ulong S(ulong X)
    {
        ulong res = 0;
        ulong tempX = X;
        for (int i = 0; i < 8; i++)
        {
            var first = (byte)BitOperations.RotateRight(tempX, 56 - 8 * i);
            res ^= SBox[first];
            res = BitOperations.RotateLeft(res, 8 * (7 - i));
        }

        return res;
    }

    //P(X)
    private static ulong P(ulong X)
    {
        ulong res = 0;
        byte[] y = new byte[8];
        byte[] x = ULongToBytes(X);
        y[7] = (byte)(x[7] ^ x[3]);
        y[6] = (byte)(x[6] ^ x[2]);
        y[5] = (byte)(x[5] ^ x[1]);
        y[4] = (byte)(x[4] ^ x[0]);
        y[3] = (byte)(x[3] ^ x[5]);
        y[2] = (byte)(x[2] ^ x[4]);
        y[1] = (byte)(x[1] ^ x[7]);
        y[0] = (byte)(x[0] ^ x[6]);
        y[7] = (byte)(y[7] ^ y[2]);
        y[6] = (byte)(y[6] ^ y[1]);
        y[5] = (byte)(y[5] ^ y[0]);
        y[4] = (byte)(y[4] ^ y[3]);
        y[3] = (byte)(y[3] ^ y[7]);
        y[7] = (byte)(y[2] ^ y[6]);
        y[1] = (byte)(y[1] ^ y[5]);
        y[0] = (byte)(y[0] ^ y[4]);
        res = BytesToULong(y);

        return res;
    }

    //циклический сдвиг влево на 8 бит
    private static ulong BRL(ulong a)
    {
        //обычный сдвиг влево на 8 бит
        ulong b = BitOperations.RotateLeft(a, 8);
        //в с хранятся первые 8 бит, а сдвинуты в последние 8 разрядов
        ulong c = BitOperations.RotateRight(0xFF00000000000000 & a, 56);

        return b ^ c;
    }

    //раундовая функция
    public static ulong F(ulong R, ulong[] roundk)
    {
        return BRL(S(P(S(R ^ roundk[0])) ^ roundk[1]));
    }

    public static void G(ulong[] X, ulong U, ref ulong[] L, ref ulong[] Y, out ulong V)
    {
        L = new ulong[4];
        Y = new ulong[4];

        for (int i = 0; i < 4; ++i)
            Y[i] = P(S(X[i]));

        L[0] = Y[0] ^ P(S(U));
        for (int j = 1; j < 4; ++j)
        {
            L[j] = P(S(L[j - 1])) ^ Y[j];
        }

        V = L[3];
    }
}