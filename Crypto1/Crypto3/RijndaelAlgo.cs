using System;
using System.Collections.Generic;
using AES;
using Crypto3.Math;

namespace Crypto3
{
    public class RijndaelAlgo
    {
        public static byte[] FitBlocks(byte[] bytes)
        {
            byte[] result = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i += 4)
            {
                byte[] column = new byte[4];
                for (int j = 0; j < 4; j++)
                {
                    column[j] = bytes[i + j];
                }
                result = SetColumn(result, i / 4, column);
            }

            return result;
        }

        public static byte[] InvFitBlocks(byte[] bytes)
        {
            byte[] result = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length / 4; i++)
            {
                byte[] column = GetColumn(bytes, i);
                for (int j = 0; j < 4; j++)
                {
                    result[4 * i + j] = column[j];
                }
            }

            return result;
        }

        #region Rounds
        public static byte[] Round(byte[] state, byte[] roundKey)
        {

            state = ByteSub(state);

            state = ShiftRow(state);

            state = MixColumns(state);

            state = AddRoundKey(state, roundKey);

            return state;
        }

        public static byte[] InvRound(byte[] state, byte[] roundKey)
        {

            state = InvShiftRow(state);

            state = InvByteSub(state);

            state = AddRoundKey(state, roundKey);

            state = InvMixColumns(state);

            return state;
        }

        public static byte[] FinalRound(byte[] state, byte[] roundKey)
        {

            state = ByteSub(state);

            state = ShiftRow(state);

            state = AddRoundKey(state, roundKey);

            return state;
        }

        public static byte[] InvFinalRound(byte[] state, byte[] roundKey)
        {
            state = InvByteSub(state);
            state = InvShiftRow(state);
            state = AddRoundKey(state, roundKey);
            return state;
        }

        #endregion


        public static byte[] SelectRoundKey(byte[] keys, int i)
        {
            byte[] result = new byte[SizeValues.Nb * 4];
            for (int j = 0; j < SizeValues.Nb; j++)
            {
                result = SetColumn(result, j, GetColumn(keys, SizeValues.Nb * i + j));
            }
            return result;
        }



        #region MainFunctions
        public static byte[] ByteSub(byte[] State)
        {
            for (int i = 0; i < State.Length; i++)
            {
                State[i] = global::AES.Sbox.GenerateSBox(State[i]);
            }
            return State;
        }

        public static byte[] InvByteSub(byte[] State)
        {
            for (int i = 0; i < State.Length; i++)
            {
                State[i] = global::AES.Sbox.GenerateInvSBox(State[i]);
            }
            return State;
        }

        public static byte[] ShiftRow(byte[] state)
        {
            state = LeftShiftRow(state, 1, 1);
            if (SizeValues.Nb == 8)
            {
                state = LeftShiftRow(state, 2, 3);
                state = LeftShiftRow(state, 3, 4);
            }
            else
            {
                state = LeftShiftRow(state, 2, 2);
                state = LeftShiftRow(state, 3, 3);
            }

            return state;

        }

        public static byte[] InvShiftRow(byte[] state)
        {
            state = RightShiftRow(state, 1, 1);
            if (SizeValues.Nb == 8)
            {
                state = RightShiftRow(state, 2, 3);
                state = RightShiftRow(state, 3, 4);
            }
            else
            {
                state = RightShiftRow(state, 2, 2);
                state = RightShiftRow(state, 3, 3);
            }

            return state;

        }



        public static byte[] AddRoundKey(byte[] state, byte[] key)
        {
            if (state.Length != key.Length) throw new ArgumentException();
            for (int i = 0; i < state.Length; i++)
            {
                state[i] ^= key[i];
            }
            return state;
        }
        #endregion


        #region ColumnOperations
        public static byte[] MixColumn(byte[] stateColumn)
        {
            if (stateColumn.Length != 4) throw new ArgumentException("State must be column, size is 4");

            List<GF> B = new List<GF>();
            B.Add(new GF(stateColumn[0], SizeValues.BasePolynome));
            B.Add(new GF(stateColumn[1], SizeValues.BasePolynome));
            B.Add(new GF(stateColumn[2], SizeValues.BasePolynome));
            B.Add(new GF(stateColumn[3], SizeValues.BasePolynome));

            stateColumn[0] = (byte)(B[0].Multiply(2).GetPolynom() ^ B[1].Multiply(3).GetPolynom() ^ B[2].Multiply(1).GetPolynom() ^ B[3].Multiply(1).GetPolynom());
            stateColumn[1] = (byte)(B[0].Multiply(1).GetPolynom() ^ B[1].Multiply(2).GetPolynom() ^ B[2].Multiply(3).GetPolynom() ^ B[3].Multiply(1).GetPolynom());
            stateColumn[2] = (byte)(B[0].Multiply(1).GetPolynom() ^ B[1].Multiply(1).GetPolynom() ^ B[2].Multiply(2).GetPolynom() ^ B[3].Multiply(3).GetPolynom());
            stateColumn[3] = (byte)(B[0].Multiply(3).GetPolynom() ^ B[1].Multiply(1).GetPolynom() ^ B[2].Multiply(1).GetPolynom() ^ B[3].Multiply(2).GetPolynom());
            return stateColumn;
        }

        public static byte[] SubWord(byte[] column)
        {
            for (int i = 0; i < 4; i++)
            {
                column[i] = global::AES.Sbox.GenerateSBox(column[i]);
            }
            return column;
        }

        public static byte[] RotWord(byte[] column)
        {
            byte[] result = new byte[4];
            result[0] = column[1];
            result[1] = column[2];
            result[2] = column[3];
            result[3] = column[0];

            return result;
        }
        public static byte[] MixColumns(byte[] state)
        {
            for (int i = 0; i < SizeValues.Nb; i++)
            {
                byte[] column = GetColumn(state, i);
                byte[] mixed = MixColumn(column);
                state = SetColumn(state, i, mixed);
            }
            return state;
        }

        public static byte[] InvMixColumns(byte[] state)
        {
            for (int i = 0; i < SizeValues.Nb; i++)
            {
                byte[] column = GetColumn(state, i);
                byte[] mixed = InvMixColumn(column);
                state = SetColumn(state, i, mixed);
            }
            return state;
        }
        public static byte[] InvMixColumn(byte[] stateColumn)
        {
            if (stateColumn.Length != 4) throw new ArgumentException("State must be column, size is 4");

            List<GF> B = new List<GF>();
            B.Add(new GF(stateColumn[0], SizeValues.BasePolynome));
            B.Add(new GF(stateColumn[1], SizeValues.BasePolynome));
            B.Add(new GF(stateColumn[2], SizeValues.BasePolynome));
            B.Add(new GF(stateColumn[3], SizeValues.BasePolynome));

            stateColumn[0] = (byte)(B[0].Multiply(0x0e).GetPolynom() ^ B[1].Multiply(0x0b).GetPolynom() ^ B[2].Multiply(0x0d).GetPolynom() ^ B[3].Multiply(0x09).GetPolynom());
            stateColumn[1] = (byte)(B[0].Multiply(0x09).GetPolynom() ^ B[1].Multiply(0x0e).GetPolynom() ^ B[2].Multiply(0x0b).GetPolynom() ^ B[3].Multiply(0x0d).GetPolynom());
            stateColumn[2] = (byte)(B[0].Multiply(0x0d).GetPolynom() ^ B[1].Multiply(0x09).GetPolynom() ^ B[2].Multiply(0x0e).GetPolynom() ^ B[3].Multiply(0x0b).GetPolynom());
            stateColumn[3] = (byte)(B[0].Multiply(0x0b).GetPolynom() ^ B[1].Multiply(0x0d).GetPolynom() ^ B[2].Multiply(0x09).GetPolynom() ^ B[3].Multiply(0x0e).GetPolynom());
            return stateColumn;
        }
        public static byte[] XorColumn(byte[] left, byte[] right)
        {
            if (left.Length != 4 || right.Length != 4) throw new ArgumentOutOfRangeException();
            byte[] result = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                result[i] = (byte)(left[i] ^ right[i]);

            }
            return result;
        }

        public static byte[] GetColumn(byte[] array, int columnIndex)
        {
            if (array.Length % 4 != 0 || array.Length / 4 <= columnIndex) throw new ArgumentOutOfRangeException();
            int length = array.Length / 4;
            byte[] result = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                result[i] = array[i * length + columnIndex];
            }

            return result;
        }

        public static byte[] SetColumn(byte[] array, int columnIndex, byte[] column)
        {
            if (array.Length % 4 != 0 || array.Length / 4 <= columnIndex) throw new ArgumentOutOfRangeException();
            int length = array.Length / 4;

            for (int i = 0; i < 4; i++)
            {
                array[i * length + columnIndex] = column[i];
            }

            return array;
        }

        #endregion
        public static byte[] RightShiftRow(byte[] state, int row, int n)
        {
            n %= SizeValues.Nb;
            byte[] result = (byte[])state.Clone();
            int leftIndex = row * SizeValues.Nb;
            int rightIndex = leftIndex + SizeValues.Nb - 1;
            for (int i = leftIndex; i <= rightIndex; i++)
            {
                if (i + n > rightIndex)
                {
                    result[leftIndex + ((i + n) % SizeValues.Nb)] = state[i];
                }
                else
                {
                    result[i + n] = state[i];
                }
            }

            return result;
        }

        public static byte[] LeftShiftRow(byte[] state, int row, int n)
        {
            n %= SizeValues.Nb;
            byte[] result = (byte[])state.Clone();
            int leftIndex = row * SizeValues.Nb;
            int rightIndex = leftIndex + SizeValues.Nb - 1;
            for (int i = leftIndex; i <= rightIndex; i++)
            {
                if (i + n > rightIndex)
                {
                    result[i] = state[leftIndex + ((i + n) % SizeValues.Nb)];
                }
                else
                {
                    result[i] = state[i + n];
                }
            }

            return result;
        }
    }
}
