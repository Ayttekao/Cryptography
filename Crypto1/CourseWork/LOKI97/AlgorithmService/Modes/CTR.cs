using System;
using System.Collections.Generic;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public class CTR : EncryptionModeBase
    {
        public override byte[] Encrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            throw new NotImplementedException();
        }

        public override byte[] Decrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            throw new NotImplementedException();
        }
    }
}