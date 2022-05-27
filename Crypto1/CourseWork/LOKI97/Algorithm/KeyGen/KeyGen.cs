using System;
using CourseWork.LOKI97.Algorithm.EncryptionTransformation;

namespace CourseWork.LOKI97.Algorithm.KeyGen
{
    public class KeyGen : IKeyGen
    {
        private const UInt32 NumSubKeys = 48;
        private const UInt64 Delta = 0x9E3779B97F4A7C15;
        
        public Object MakeKey(Byte[] k, IEncryptionTransformation encryptionTransformation)
        {
            UInt64[] SK = new UInt64[NumSubKeys];    // array of subkeys
    
            UInt64 deltan = Delta;            // multiples of delta
    
            var i = 0;                // index into key input
            UInt64 k1;            // key schedule 128-bit entities
            UInt64 k2;
            UInt64 k3;
            UInt64 k4;
            UInt64 fOut;                // fn f output value for debug
    
            // pack key into 128-bit entities: k4, k3, k2, k1
            k4 = (UInt64)((k[i++] & 0xFFL) << 56 | (k[i++] & 0xFFL) << 48 |
                          (k[i++] & 0xFFL) << 40 | (k[i++] & 0xFFL) << 32 |
                          (k[i++] & 0xFFL) << 24 | (k[i++] & 0xFFL) << 16 |
                          (k[i++] & 0xFFL) << 8 | (k[i++] & 0xFFL));
            k3 = (UInt64)((k[i++] & 0xFFL) << 56 | (k[i++] & 0xFFL) << 48 |
                          (k[i++] & 0xFFL) << 40 | (k[i++] & 0xFFL) << 32 |
                          (k[i++] & 0xFFL) << 24 | (k[i++] & 0xFFL) << 16 |
                          (k[i++] & 0xFFL) << 8 | (k[i++] & 0xFFL));
    
            if (k.Length == 16)
            {   // 128-bit key - call fn f twice to gen 256 bits
                k2 = encryptionTransformation.Compute(k3, k4);
                k1 = encryptionTransformation.Compute(k4, k3);
            } 
            else
            {                // 192 or 256-bit key - pack k2 from key data
                k2 = (UInt64)((k[i++] & 0xFFL) << 56 | (k[i++] & 0xFFL) << 48 |
                              (k[i++] & 0xFFL) << 40 | (k[i++] & 0xFFL) << 32 |
                              (k[i++] & 0xFFL) << 24 | (k[i++] & 0xFFL) << 16 |
                              (k[i++] & 0xFFL) << 8 | (k[i++] & 0xFFL));
                if (k.Length == 24) // 192-bit key - call fn f once to gen 256 bits
                    k1 = encryptionTransformation.Compute(k4, k3);
                else                // 256-bit key - pack k1 from key data
                    k1 = (UInt64)((k[i++] & 0xFFL) << 56 | (k[i++] & 0xFFL) << 48 |
                                  (k[i++] & 0xFFL) << 40 | (k[i++] & 0xFFL) << 32 |
                                  (k[i++] & 0xFFL) << 24 | (k[i++] & 0xFFL) << 16 |
                                  (k[i++] & 0xFFL) << 8 | (k[i++] & 0xFFL));
            }
    
            // iterate over all LOKI97 rounds to generate the required subkeys
            for (i = 0; i < NumSubKeys; i++)
            {
                fOut = encryptionTransformation.Compute(k1 + k3 + deltan, k2);
                SK[i] = k4 ^ fOut;        // compute next subkey value using fn f
                k4 = k3;            // exchange the other words around
                k3 = k2;
                k2 = k1;
                k1 = SK[i];
                deltan += Delta;        // next multiple of delta
            }
    
            return SK;
        }
    }
}