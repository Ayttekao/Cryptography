using System;
using Crypto1.Enums;
using Crypto1.Interfaces;

namespace Crypto1
{
    public class SymmetricCypherAlgorithm : ICypherAlgorithm
    {
        public SymmetricCypherAlgorithm(Byte[] key,
            EncryptionModes mode,
            Byte[] initializationVector = null,
            params object[] list)
        {
            
        }
        
        private static byte[] PadBuffer(Byte[] buf, Int32 padFrom, Int32 padTo, Paddings padding = Paddings.PKCS7) 
         {
            if ((padTo < buf.Length) | (padTo - padFrom > 255))
            {
                return buf;
            }
            
            var b = new Byte[padTo];
            Buffer.BlockCopy(buf, 0, b, 0, padFrom);
            
            for (var count = padFrom; count < padTo; count++) 
            {
                switch(padding) 
                {
                    case Paddings.PKCS7:
                        b[count] = (Byte) (padTo - padFrom);
                        break;
                    case Paddings.NONE:
                        b[count] = 0;
                        break;
                    default:
                        return buf;
                }
            }
            return b;
        }

        public void Encrypt(Byte[] inputBlock, ref Byte[] encryptBlock)
        {
            
        }

        public void Decrypt(Byte[] inputBlock, ref Byte[] decryptBlock)
        {
            
        }
        
        public void Encrypt(String inputFile, String outputFile)
        {
            
        }

        public void Decrypt(String inputFile, String outputFile)
        {
            
        }
        
        public Byte[] Encrypt(Byte[] inputBlock)
        {
            throw new NotImplementedException();
        }

        public Byte[] Decrypt(Byte[] inputBlock)
        {
            throw new NotImplementedException();
        }
    }
}