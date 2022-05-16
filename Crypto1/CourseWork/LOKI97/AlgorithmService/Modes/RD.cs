using System;
using System.Collections.Generic;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public sealed class RD : EncryptionModeBase
    {
        public override Byte[] Encrypt(List<Byte[]> blocksList, object key, Byte[] iv)
        {
            throw new NotImplementedException();
        }

        public override Byte[] Decrypt(List<Byte[]> blocksList, object key, Byte[] iv)
        {
            throw new NotImplementedException();
        }
    }
}