using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Crypto1.Padding
{
    /*
     * 5. Добавьте в класс-контекст 3.4 из лабораторной работы 1 возможность настройки типа набивки (PKCS7, ISO 10126,
     * ANSI X.923) через объект перечисления, передаваемый в конструктор. Продемонстрируйте добавленные в реализацию
     * класса-контекста режимы набивки на алгоритмах DES и Rijndael.
     */
    
    public class Padder
    {
        private PaddingType _paddingType;
        private Int32 _blockSize;

        public Padder(PaddingType paddingType, Int32 blockSize)
        {
            _paddingType = paddingType;
            _blockSize = blockSize;
        }

        private Byte[] PadBuffer(Byte[] buf, Int32 padFrom, Int32 padTo) 
        {
            if (padTo < buf.Length || padTo - padFrom > Byte.MaxValue || _paddingType == PaddingType.NONE)
            {
                return buf;
            }
            
            var b = new Byte[padTo];
            Buffer.BlockCopy(buf, 0, b, 0, padFrom);
            
            for (var count = padFrom; count < padTo; count++) 
            {
                if (count < padTo - 1)
                {
                    switch (_paddingType)
                    {
                        case PaddingType.PKCS7:
                            b[count] = (Byte) (padTo - padFrom);
                            break;
                        case PaddingType.ISO_10126:
                            b[count] = (Byte)RandomNumberGenerator.GetInt32(Byte.MaxValue);
                            break;
                        case PaddingType.ANSI_X_923:
                            b[count] = 0;
                            break;
                        case PaddingType.NONE:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    b[count] = (Byte)(padTo - padFrom);
                }
            }
            return b;
        }

        public Byte[] PadBuffer(Byte[] buf) {
            var extraBlock = (buf.Length % _blockSize == 0) && _paddingType == PaddingType.NONE ? 0 : 1;
            return PadBuffer(buf, buf.Length, ((buf.Length / _blockSize) + extraBlock) * _blockSize);
        }

        public Byte[] RemovePadding(List<Byte[]> blocks)
        {
            var array = blocks.SelectMany(x => x.ToArray()).ToArray();
            var extraBlocks = array[array.Length - 1];
            var result = new Byte[array.Length - extraBlocks];
            Array.Copy(array, result, result.Length);

            return result;
        }
    }
}