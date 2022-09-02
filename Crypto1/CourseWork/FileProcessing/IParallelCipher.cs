using System;
using System.Threading.Tasks;
using CourseWork.SymmetricAlgorithms.AlgorithmService.Modes;

namespace CourseWork.FileProcessing
{
    public interface IParallelCipher
    {
        public Task<Byte[]> Encrypt(String filePath, EncryptionMode encryptionMode);
        public Task<Byte[]> Decrypt(Byte[] inputBuffer, EncryptionMode encryptionMode);
        public Task Decrypt(String path, Byte[] inputBuffer, EncryptionMode encryptionMode);
    }
}