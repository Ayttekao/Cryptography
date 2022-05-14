using System;
using CourseWork.LOKI97.AlgorithmService.Enums;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public class ModeFactory
    {
        public static EncryptionModeBase CreateEncryptionMode(EncryptionMode encryptionMode)
        {
            return encryptionMode switch
            {
                EncryptionMode.ECB => new ECB(),
                EncryptionMode.CBC => new CBC(),
                EncryptionMode.CFB => new CFB(),
                EncryptionMode.OFB => new OFB(),
                _ => throw new ArgumentException("Unexpected value: " + encryptionMode)
            };
        }
    }
}