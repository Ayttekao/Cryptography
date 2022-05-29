using System;
using System.Text;

namespace CourseWork.LOKI97.AlgorithmService.Modes
{
    public class ModeFactory
    {
        //remove static 
        public static EncryptionModeBase CreateEncryptionMode(EncryptionMode encryptionMode, params Object[] list)
        {
            //dict enums to class
            return encryptionMode switch
            {
                EncryptionMode.ECB => new ECB(),
                EncryptionMode.CBC => new CBC(),
                EncryptionMode.CFB => new CFB(),
                EncryptionMode.OFB => new OFB(),
                EncryptionMode.CTR => new CTR(),
                EncryptionMode.RD => new RD(),
                EncryptionMode.RDH => new RDH(Encoding.Default.GetBytes("list.First() as Byte[]")),
                _ => throw new ArgumentException("Unexpected value: " + encryptionMode)
            };
        }
    }
}