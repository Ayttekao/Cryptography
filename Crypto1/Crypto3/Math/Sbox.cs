using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto3.Math;

namespace AES
{
    public class Sbox
    {
        public static byte GenerateSBox(byte n)
        {
            byte b = new GF(n).Inverse().GetPolynom();
            return (byte)(b ^ (RotateLeft(b, 1) ^ RotateLeft(b, 2) ^ RotateLeft(b, 3) ^ RotateLeft(b, 4) ^ 0x63));
        }

        public static byte GenerateInvSBox(byte n)
        {

            return new GF((byte)(RotateLeft(n, 1) ^ RotateLeft(n, 3) ^ RotateLeft(n, 6) ^ 0x5)).Inverse().GetPolynom();
        }


        #region Utils

        private static byte RotateLeft(byte x, byte n)
        {
            return (byte)((x << n) | (x >> (8 - n)));
        }

        private static byte RotateRight(byte x, byte n)
        {
            return (byte)((x >> n) | (x << (8 - n)));
        }
        #endregion

    }
}



