using System;
using System.Linq;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public class ModeFactory
    {
        public static EncryptionModeBase CreateEncryptionMode(EncryptionMode encryptionMode, params Object[] list)
        {
            return encryptionMode switch
            {
                EncryptionMode.ECB => new ECB(),
                EncryptionMode.CBC => new CBC(),
                EncryptionMode.CFB => new CFB(),
                EncryptionMode.OFB => new OFB(),
                EncryptionMode.CTR => new CTR(),
                EncryptionMode.RD => new RD(),
                EncryptionMode.RDH => new RDH(list.First() as Byte[]),
                _ => throw new ArgumentException("Unexpected value: " + encryptionMode)
            };
        }
    }
}