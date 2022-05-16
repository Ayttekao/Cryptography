using System.Collections.Generic;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public sealed class RDH : EncryptionModeBase
    {
        public override byte[] Encrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            throw new System.NotImplementedException();
        }

        public override byte[] Decrypt(List<byte[]> blocksList, object key, byte[] iv)
        {
            throw new System.NotImplementedException();
        }
    }
}