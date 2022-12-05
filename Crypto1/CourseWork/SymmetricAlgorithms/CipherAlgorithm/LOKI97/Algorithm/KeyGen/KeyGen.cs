using System;
using CourseWork.SymmetricAlgorithms.CipherAlgorithm.LOKI97.Algorithm.EncryptionTransformation;

namespace CourseWork.SymmetricAlgorithms.CipherAlgorithm.LOKI97.Algorithm.KeyGen
{
    public class KeyGen : IKeyGen
    {
        private const UInt32 NumSubKeys = 48;
        private const UInt64 Delta = 0x9E3779B97F4A7C15;
        
        public Object MakeKey(Byte[] k, IEncryptionTransformation encryptionTransformation)
        {
            UInt64[] SK = new UInt64[NumSubKeys];
    
            UInt64 deltan = Delta;
    
            var i = 0;
            UInt64 k1;
            UInt64 k2;
            UInt64 k3;
            UInt64 k4;
            UInt64 fOut;
    
            k4 = (UInt64)((k[i++] & 0xFFL) << 56 | (k[i++] & 0xFFL) << 48 |
                          (k[i++] & 0xFFL) << 40 | (k[i++] & 0xFFL) << 32 |
                          (k[i++] & 0xFFL) << 24 | (k[i++] & 0xFFL) << 16 |
                          (k[i++] & 0xFFL) << 8 | (k[i++] & 0xFFL));
            k3 = (UInt64)((k[i++] & 0xFFL) << 56 | (k[i++] & 0xFFL) << 48 |
                          (k[i++] & 0xFFL) << 40 | (k[i++] & 0xFFL) << 32 |
                          (k[i++] & 0xFFL) << 24 | (k[i++] & 0xFFL) << 16 |
                          (k[i++] & 0xFFL) << 8 | (k[i++] & 0xFFL));
    
            if (k.Length == 16)
            {   
                k2 = encryptionTransformation.Compute(k3, k4);
                k1 = encryptionTransformation.Compute(k4, k3);
            } 
            else
            {                
                k2 = (UInt64)((k[i++] & 0xFFL) << 56 | (k[i++] & 0xFFL) << 48 |
                              (k[i++] & 0xFFL) << 40 | (k[i++] & 0xFFL) << 32 |
                              (k[i++] & 0xFFL) << 24 | (k[i++] & 0xFFL) << 16 |
                              (k[i++] & 0xFFL) << 8 | (k[i++] & 0xFFL));
                if (k.Length == 24)
                {
                    k1 = encryptionTransformation.Compute(k4, k3);
                }
                else
                {
                    k1 = (UInt64)((k[i++] & 0xFFL) << 56 | (k[i++] & 0xFFL) << 48 |
                                  (k[i++] & 0xFFL) << 40 | (k[i++] & 0xFFL) << 32 |
                                  (k[i++] & 0xFFL) << 24 | (k[i++] & 0xFFL) << 16 |
                                  (k[i++] & 0xFFL) << 8 | (k[i++] & 0xFFL));
                }
            }
    
            for (i = 0; i < NumSubKeys; i++)
            {
                fOut = encryptionTransformation.Compute(k1 + k3 + deltan, k2);
                SK[i] = k4 ^ fOut;
                k4 = k3;
                k3 = k2;
                k2 = k1;
                k1 = SK[i];
                deltan += Delta;
            }
    
            return SK;
        }
    }
}