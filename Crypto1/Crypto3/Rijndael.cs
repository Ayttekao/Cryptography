using System;
using Crypto3.Math;

namespace Crypto3
{
    public struct SizeValues
    {
        public static int Nb; //    block size / 32
        public static int Nk; //    key len / 32
        public static uint BasePolynome;
        public static int Nr;
    }
    class Rijndael: Chipher
    {
        public Rijndael(int Nb, int Nk, uint basePolynome, RoundKeysGenerator name2) : base(name2)
        {
            if (!(Nb == 4 || Nb == 6 || Nb == 8))
            {
                throw new ArgumentException("Nb error");
            }

            if (!(Nk == 4 || Nk == 6 || Nk == 8))
            {
                throw new ArgumentException("Nk error");
            }

            SizeValues.Nb = Nb;
            SizeValues.Nk = Nk;
            SizeValues.BasePolynome = basePolynome;
            if (Nk == 4 && Nb == 4) SizeValues.Nr = 10;
            else
                    if (Nk == 6 && Nb != 8) SizeValues.Nr = 12;
            else
                    if (Nk == 4 && Nb == 6) SizeValues.Nr = 12;
            else
                SizeValues.Nr = 14;
            if (!GF.CheckIrr(SizeValues.BasePolynome)) throw new ArgumentOutOfRangeException();
            
        }


        public override byte[] EncryptBlock(byte[] message) 
        {
            var enrcrypt = base.EncryptBlock(message);
            return enrcrypt;
        }

        public override byte[] DecryptBlock(byte[] message)
        {
            var DecryptBlock = base.DecryptBlock(message);
            return DecryptBlock;
        }
    }
}
