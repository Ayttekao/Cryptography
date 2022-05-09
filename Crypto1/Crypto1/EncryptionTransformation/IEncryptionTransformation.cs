using System;

namespace Crypto1.EncryptionTransformation
{
    /*
     * 3.3 - Интерфейс, предоставляющий описание функционала по выполнению шифрования и дешифрования симметричным
     * алгоритмом (параметр методов: [де]шифруемый блок (массив байтов)) с преднастроенными отдельным методом
     * раундовыми ключами (параметр метода: ключ [де]шифрования (массив байтов));
     */
    
    public interface IEncryptionTransformation
    {
        Byte[] EncryptionTransformation(Byte[] inputBlock, Byte[] roundKey);
    }
}