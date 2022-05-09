using System;

namespace Crypto1.Padding
{
    public class Padder
    {
        private PaddingType _paddingType;
        private Int32 _blockSize;

        public Padder(PaddingType paddingType, Int32 blockSize)
        {
            _paddingType = paddingType;
            _blockSize = blockSize;
        }
        
        public Byte[] PadBuffer(Byte[] buf, Int32 padFrom, Int32 padTo) 
        {
            if ((padTo < buf.Length) | (padTo - padFrom > 255))
            {
                return buf;
            }
            var b = new Byte[padTo];
            Buffer.BlockCopy(buf, 0, b, 0, padFrom);
            
            for (var count = padFrom; count < padTo; count++) 
            {
                switch(_paddingType) 
                {
                    case PaddingType.PKCS7:
                        b[count] = (Byte) (padTo - padFrom);
                        break;
                    case PaddingType.NONE:
                        b[count] = 0;
                        break;
                    default:
                        return buf;
                }
            }
            return b;
        }

        public Byte[] PadBuffer(Byte[] buf) {
            var extraBlock = (buf.Length % _blockSize == 0) && _paddingType == PaddingType.NONE ? 0 : 1;
            return PadBuffer(buf, buf.Length, ((buf.Length / _blockSize) + extraBlock) * _blockSize);
        }
    }
}