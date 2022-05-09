using System;

namespace Crypto1.CypherAlgorithm
{
    /*
     * 3.2 - Интерфейс, предоставляющий описание функционала по выполнению шифрующего преобразования
     * (параметры метода: входной блок - массив байтов, раундовый ключ - массив байтов, результат:
     * выходной блок - массив байтов);
     */
    
    public interface ICypherAlgorithm
    {
        Byte[] Encrypt(Byte[] inputBlock);
        Byte[] Decrypt(Byte[] inputBlock);
    }
}