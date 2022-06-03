using Crypto3.Interfaces;
using Crypto3.Math;

namespace Crypto3
{
    class RoundKeysGenerator : IGenerationKey
	{
		public byte[] GenerateRoundKeys(byte[] key)
        {
            byte[] rc = new byte[40];
            int keyCount = SizeValues.Nr + 1;
            int N = key.Length / 4;
            byte[] result = new byte[4 * keyCount * SizeValues.Nb];
            rc[0] = 1;
            for (int i = 1; i < 32; i++)
            {
                GF x = new GF(0b10, SizeValues.BasePolynome);
                rc[i] = x.Multiply(new GF(rc[i - 1], SizeValues.BasePolynome)).GetPolynom();
            }

            for (int i = 0; i < SizeValues.Nb * keyCount; i++)
            {
                if (i < N)
                {
                    byte[] column = RijndaelAlgo.GetColumn(key, i);
                    result = RijndaelAlgo.SetColumn(result, i, column);
                }
                else if (i >= N && i % N == 0)
                {
                    byte[] column = RijndaelAlgo.GetColumn(result, i - N);
                    byte[] prev = RijndaelAlgo.GetColumn(result, i - 1);

                    column = RijndaelAlgo.XorColumn(column, RijndaelAlgo.SubWord(RijndaelAlgo.RotWord(prev)));
                    column[0] ^= rc[i / N - 1];
                    result = RijndaelAlgo.SetColumn(result, i, column);
                }
                else if (i >= N && N > 6 && i % N == 4)
                {
                    byte[] column = RijndaelAlgo.GetColumn(result, i - N);
                    byte[] prev = RijndaelAlgo.GetColumn(result, i - 1);
                    column = RijndaelAlgo.XorColumn(column, RijndaelAlgo.SubWord(prev));
                    result = RijndaelAlgo.SetColumn(result, i, column);
                }
                else
                {
                    byte[] column = RijndaelAlgo.GetColumn(result, i - N);
                    byte[] prev = RijndaelAlgo.GetColumn(result, i - 1);
                    column = RijndaelAlgo.XorColumn(column, prev);
                    result = RijndaelAlgo.SetColumn(result, i, column);
                }
            }

            return result;
        }

	}
}
