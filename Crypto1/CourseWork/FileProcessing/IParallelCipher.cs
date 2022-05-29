using System;
using CourseWork.LOKI97.AlgorithmService.Modes;

namespace CourseWork.FileProcessing
{
    public interface IParallelCipher
    {
        public Byte[] Encrypt(String filePath, EncryptionMode encryptionMode);
        public Byte[] Decrypt(Byte[] inputBuffer, EncryptionMode encryptionMode);
    }
}