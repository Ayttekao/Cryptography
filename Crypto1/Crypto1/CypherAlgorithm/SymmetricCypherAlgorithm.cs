using System;
using Crypto1.CipherModes;
using Crypto1.Padding;

namespace Crypto1.CypherAlgorithm
{
    /*
     * 3.4 - Класс-контекст, предоставляющий объектный функционал по выполнению шифрования и дешифрования симметричным
     * алгоритмом (реализацией интерфейса из п. 3) с поддержкой одного из режимов шифрования (задаётся перечислением):
     * ECB, CBC, CFB, OFB, CTR, RD, RD+H. Параметры конструктора класса: ключ шифрования, режим шифрования
     * (объект перечисления), вектор инициализации (опционально), дополнительные параметры для указанного режима
     * (список аргументов переменной длины). Параметры методов шифрования/дешифрования: данные для шифрования
     * (массив байтов произвольной длины и ссылка на результирующий массив байтов, либо путь к файлу со входными
     * данными и путь к файлу с результатом [де]шифрования). Где возможно, реализуйте распараллеливание вычислений.
     * Шифрование должно производиться асинхронно. При реализации используйте тип набивки (padding) PKCS7.
     */
    
    public class SymmetricCypherAlgorithm
    {
        private Int32 _blockSize = 8;
        private readonly CipherModeBase _cipherModeBase;
        private ICypherAlgorithm Algorithm { get; set; }
        
        public SymmetricCypherAlgorithm(CipherMode cipherMode, Byte[] initializationVector,
            ICypherAlgorithm algorithm, String valueForHash = null)
        {
            Algorithm = algorithm;

            _cipherModeBase = cipherMode switch
            {
                CipherMode.ECB => new ECB(Algorithm, initializationVector, PaddingType.ANSI_X_923, _blockSize),
                CipherMode.CBC => new CBC(Algorithm, initializationVector, PaddingType.ANSI_X_923, _blockSize),
                CipherMode.CFB => new CFB(Algorithm, initializationVector, PaddingType.ANSI_X_923, _blockSize),
                CipherMode.OFB => new OFB(Algorithm, initializationVector, PaddingType.ANSI_X_923, _blockSize),
                CipherMode.CTR => new CTR(Algorithm, initializationVector, PaddingType.ANSI_X_923, _blockSize),
                CipherMode.RD => new RD(Algorithm, initializationVector, PaddingType.ANSI_X_923, _blockSize),
                CipherMode.RDH => new RDH(Algorithm, initializationVector, valueForHash, PaddingType.ANSI_X_923, _blockSize),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public SymmetricCypherAlgorithm(Byte[] key, CipherMode mode, Byte[] initializationVector, 
            params object[] list)
        {
            
        }

        public Byte[] Encrypt(Byte[] inputBlock)
        {
            return _cipherModeBase.Encrypt(inputBlock);
        }

        public Byte[] Decrypt(Byte[] inputBlock)
        {
            return _cipherModeBase.Decrypt(inputBlock);
        }

        public void Encrypt(Byte[] inputBlock, ref Byte[] encryptBlock)
        {
            encryptBlock = encryptBlock == null
                ? throw new ArgumentNullException(nameof(encryptBlock))
                : _cipherModeBase.Encrypt(inputBlock);
        }

        public void Decrypt(Byte[] inputBlock, ref Byte[] decryptBlock)
        {
            decryptBlock = decryptBlock == null
                ? throw new ArgumentNullException(nameof(decryptBlock))
                : _cipherModeBase.Decrypt(inputBlock);
        }

        public void Encrypt(String inputFile, String outputFile)
        {
            
        }

        public void Decrypt(String inputFile, String outputFile)
        {
            
        }
    }
}