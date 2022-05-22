using System;

namespace CourseWork.LOKI97.Algorithm.RoundKey
{
    public class RoundKeyGen : IRoundKeyGen
    {
        private readonly UInt32 NUM_SUBKEYS = 48;
        private readonly UInt64 DELTA = 0x9E3779B97F4A7C15;
        
        public object MakeKey(byte[] k)
        {
            UInt64[] SK = new UInt64[NUM_SUBKEYS];
    
            UInt64 deltan = DELTA;
    
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
                k2 = Cipher.Compute(k3, k4);
                k1 = Cipher.Compute(k4, k3);
            } 
            else
            {
                k2 = (UInt64)((k[i++] & 0xFFL) << 56 | (k[i++] & 0xFFL) << 48 |
                              (k[i++] & 0xFFL) << 40 | (k[i++] & 0xFFL) << 32 |
                              (k[i++] & 0xFFL) << 24 | (k[i++] & 0xFFL) << 16 |
                              (k[i++] & 0xFFL) << 8 | (k[i++] & 0xFFL));
                if (k.Length == 24)
                {
                    k1 = Cipher.Compute(k4, k3);
                }
                else
                {
                    k1 = (UInt64)((k[i++] & 0xFFL) << 56 | (k[i++] & 0xFFL) << 48 |
                                  (k[i++] & 0xFFL) << 40 | (k[i++] & 0xFFL) << 32 |
                                  (k[i++] & 0xFFL) << 24 | (k[i++] & 0xFFL) << 16 |
                                  (k[i++] & 0xFFL) << 8 | (k[i++] & 0xFFL));
                }
            }
    
            for (i = 0; i < NUM_SUBKEYS; i++)
            {
                fOut = Cipher.Compute(k1 + k3 + deltan, k2);
                SK[i] = k4 ^ fOut;
                k4 = k3;
                k3 = k2;
                k2 = k1;
                k1 = SK[i];
                deltan += DELTA;
            }
    
            return SK;
        }
    }
}