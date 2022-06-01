using System;
using System.Text;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;
using CourseWork.LOKI97.AlgorithmService.Modes;

namespace CourseWork.Template
{
    public class CipherTemplateFactory
    {
        public CipherTemplate Create(ICipherAlgorithm cipherAlgorithm, EncryptionMode encryptionMode, params Object[] list)
        {
            return encryptionMode switch
            {
                EncryptionMode.ECB => new CipherECB(cipherAlgorithm),
                EncryptionMode.CBC => new CipherCBC(cipherAlgorithm),
                EncryptionMode.CFB => new CipherCFB(cipherAlgorithm),
                EncryptionMode.OFB => new CipherOFB(cipherAlgorithm),
                EncryptionMode.CTR => new CipherCTR(cipherAlgorithm),
                EncryptionMode.RD => new CipherRD(cipherAlgorithm),
                EncryptionMode.RDH => new CipherRDH(cipherAlgorithm, /*(Byte[])list.First()*/ Encoding.Default.GetBytes("list.First() as Byte[]")),
                _ => throw new ArgumentException("Unexpected value: " + encryptionMode)
            };
        }
    }
}