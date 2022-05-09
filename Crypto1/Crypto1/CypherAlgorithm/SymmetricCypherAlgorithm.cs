using System;
using Crypto1.CipherModes;

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

            switch (cipherMode)
            {
                case CipherMode.ECB:
                {
                    _cipherModeBase = new ECB(Algorithm, initializationVector);
                    break;
                }
                case CipherMode.CBC:
                {
                    _cipherModeBase = new CBC(Algorithm, initializationVector);
                    break;
                }
                case CipherMode.CFB:
                {
                    _cipherModeBase = new CFB(Algorithm, initializationVector);
                    break;
                }
                case CipherMode.OFB:
                {
                    _cipherModeBase = new OFB(Algorithm, initializationVector);
                    break;
                }
                case CipherMode.CTR:
                {
                    _cipherModeBase = new CTR(Algorithm, initializationVector);
                    break;
                }
                case CipherMode.RD:
                {
                    _cipherModeBase = new RD(Algorithm, initializationVector);
                    break;
                }
                case CipherMode.RDH:
                {
                    _cipherModeBase = new RDH(Algorithm, initializationVector, valueForHash);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public SymmetricCypherAlgorithm(Byte[] key, CipherMode mode, Byte[] initializationVector, 
            params object[] list)
        {
            
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
        
        public Byte[] Encrypt(Byte[] inputBlock)
        {
            return _cipherModeBase.Encrypt(inputBlock);
        }

        public Byte[] Decrypt(Byte[] inputBlock)
        {
            return _cipherModeBase.Decrypt(inputBlock);
        }
    }
}