using System;

namespace Crypto1.CipherAlgorithm
{
    /*
     * 3.2 - Интерфейс, предоставляющий описание функционала по выполнению шифрующего преобразования
     * (параметры метода: входной блок - массив байтов, раундовый ключ - массив байтов, результат:
     * выходной блок - массив байтов);
     */
    
    public interface ICipherAlgorithm
    {
        Byte[] Encrypt(Byte[] inputBlock);
        Byte[] Decrypt(Byte[] inputBlock);
    }
}