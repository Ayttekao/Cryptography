namespace CourseWork.SymmetricAlgorithms.TwoFish.Algorithm;

public static class SBoxesGeneration
{
    private static byte[,,] Qbox = {
        {
            { 0x8, 0x1, 0x7, 0xD, 0x6, 0xF, 0x3, 0x2, 0x0, 0xB, 0x5, 0x9, 0xE, 0xC, 0xA, 0x4 },
            { 0xE, 0XC, 0XB, 0X8, 0X1, 0X2, 0X3, 0X5, 0XF, 0X4, 0XA, 0X6, 0X7, 0X0, 0X9, 0XD },
            { 0XB, 0XA, 0X5, 0XE, 0X6, 0XD, 0X9, 0X0, 0XC, 0X8, 0XF, 0X3, 0X2, 0X4, 0X7, 0X1 },
            { 0XD, 0X7, 0XF, 0X4, 0X1, 0X2, 0X6, 0XE, 0X9, 0XB, 0X3, 0X0, 0X8, 0X5, 0XC, 0XA }
        },
        {
            { 0X2, 0X8, 0XB, 0XD, 0XF, 0X7, 0X6, 0XE, 0X3, 0X1, 0X9, 0X4, 0X0, 0XA, 0XC, 0X5 },
            { 0X1, 0XE, 0X2, 0XB, 0X4, 0XC, 0X3, 0X7, 0X6, 0XD, 0XA, 0X5, 0XF, 0X9, 0X0, 0X8 },
            { 0X4, 0XC, 0X7, 0X5, 0X1, 0X6, 0X9, 0XA, 0X0, 0XE, 0XD, 0X8, 0X2, 0XB, 0X3, 0XF },
            { 0xB, 0X9, 0X5, 0X1, 0XC, 0X3, 0XD, 0XE, 0X6, 0X4, 0X7, 0XF, 0X2, 0X0, 0X8, 0XA }
        }
    };
    
    public static uint Sbox(int i, uint x)
    {
        byte a0, b0, a1, b1, a2, b2, a3, b3, a4, b4;

        a0 = (byte)((x >> 4) & 15);
        b0 = (byte)((x) & 15);

        a1 = (byte)(a0 ^ b0);
        b1 = (byte)((a0 ^ ((b0 << 3) | (b0 >> 1)) ^ (a0 << 3)) & 15);

        a2 = Qbox[i, 0, a1];
        b2 = Qbox[i, 1, b1];

        a3 = (byte)(a2 ^ b2);
        b3 = (byte)((a2 ^ ((b2 << 3) | (b2 >> 1)) ^ (a2 << 3)) & 15);

        a4 = Qbox[i, 2, a3];
        b4 = Qbox[i, 3, b3];

        uint y = (uint)(b4 << 4) + a4;

        return y;
    }
}