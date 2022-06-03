using System;
using Crypto3.Interfaces;

namespace Crypto3
{
    class Chipher: IChipher
    {
        IGenerationKey keyGenerator;
        
        byte[] keys;
        public Chipher(IGenerationKey key)
        {
            keyGenerator = key;
        }
        
        public virtual byte[] EncryptBlock(byte[] message)
        {
            if (SizeValues.Nb * 4 != message.Length) throw new ArgumentException();
            message = RijndaelAlgo.FitBlocks(message);
            message = RijndaelAlgo.AddRoundKey(message, RijndaelAlgo.SelectRoundKey(keys, 0));
            for (int i = 1; i < SizeValues.Nr; i++)
            {
                message = RijndaelAlgo.Round(message, RijndaelAlgo.SelectRoundKey(keys, i));
            }
            message = RijndaelAlgo.FinalRound(message, RijndaelAlgo.SelectRoundKey(keys, SizeValues.Nr));

            return RijndaelAlgo.InvFitBlocks(message);
        }

        public virtual byte[] DecryptBlock(byte[] message)
        {
            if (SizeValues.Nb * 4 != message.Length) throw new ArgumentException();
            message = RijndaelAlgo.FitBlocks(message);
            message = RijndaelAlgo.AddRoundKey(message, RijndaelAlgo.SelectRoundKey(keys, SizeValues.Nr));
            for (int i = SizeValues.Nr - 1; i > 0; i--)
            {
                message = RijndaelAlgo.InvRound(message, RijndaelAlgo.SelectRoundKey(keys, i));
            }
            message = RijndaelAlgo.InvFinalRound(message, RijndaelAlgo.SelectRoundKey(keys, 0));

            return RijndaelAlgo.InvFitBlocks(message);
        }

        public void SetKey(byte[] key)
        {
            if (SizeValues.Nk * 4 != key.Length) throw new ArgumentException();
            keys = keyGenerator.GenerateRoundKeys(key);
        }
    }
}
