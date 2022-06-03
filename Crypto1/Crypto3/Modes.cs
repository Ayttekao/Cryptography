using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Crypto3.Interfaces;
using static Crypto3.Padder;

namespace Crypto3
{
    class Modes
    {
        public enum EncryptionMode { ECB, CBC, CFB, OFB, CTR, RD, RDH };
        private readonly EncryptionMode _encryptionMode;
        public IChipher algorithm;
        private byte[] _initializationVector;
        static int BlockSize = SizeValues.Nb * 4;
        private String param;
        private Padder padder;

        public Modes(EncryptionMode mode, byte[] vector, PaddingType padding)
        {
            padder = new Padder(padding, BlockSize);
            _encryptionMode = mode;
            _initializationVector = vector;
        }

        public Modes(EncryptionMode mode, byte[] vector, String str, PaddingType padding)
        {
            padder = new Padder(padding, BlockSize);
            _encryptionMode = mode;
            _initializationVector = vector;
            param = str;
        }

        private List<byte[]> GetListFromArrayWithBlockSizeLength(Byte[] result)
        {
            var resultList = new List<Byte[]>();

            for (var i = 0; i < result.Length / BlockSize; i++)
            {
                resultList.Add(new byte[BlockSize]);
                for (var j = 0; j < BlockSize; j++)
                {
                    resultList[i][j] = result[i * BlockSize + j];
                }
            }

            return resultList;
        }
        public byte[] EncryptBlock(byte[] data)
        {
            var res = padder.PadBuffer(data);
            List<byte[]> blocks = new List<byte[]>(); //Enumerable.Repeat(default(Byte[]), res.Length / BlockSize).ToList(); ;
            for (int i = 0; i < res.Length / BlockSize; i++)
            {
                blocks.Add(new Byte[BlockSize]);
            }
            switch (_encryptionMode)
            {
                case EncryptionMode.ECB:
                    {
                        List<byte[]> blockList = GetListFromArrayWithBlockSizeLength(res);

                        for (int i = 0; i < res.Length / BlockSize; i++)
                        {
                            var arr = algorithm.EncryptBlock(blockList[i]);
                            Array.Copy(arr, 0, blocks[i], 0, BlockSize);
                            //blocks[i] = algorithm.EncryptBlock(blockList[i]);
                        }
                        break;
                    }

                case EncryptionMode.CBC:
                    {
                        byte[] prevBlock = new byte[BlockSize];
                        byte[] curBlock = new byte[BlockSize];
                        Array.Copy(_initializationVector, prevBlock, prevBlock.Length);

                        for (int i = 0; i < res.Length / BlockSize; i++)
                        {
                            Array.Copy(res, i * BlockSize, curBlock, 0, BlockSize);
                            blocks[i] = algorithm.EncryptBlock(XOR(curBlock, prevBlock));
                            Array.Copy(blocks[i], prevBlock, BlockSize);
                        }
                        break;
                    }

                case EncryptionMode.CFB:
                    {
                        byte[] prevBlock = new byte[BlockSize];
                        byte[] curBlock = new byte[BlockSize];
                        Array.Copy(_initializationVector, prevBlock, prevBlock.Length);
                        for (int i = 0; i < res.Length / BlockSize; i++)
                        {
                            Array.Copy(res, i * BlockSize, curBlock, 0, BlockSize);
                            blocks[i] = XOR(algorithm.EncryptBlock(prevBlock), curBlock);
                            Array.Copy(blocks[i], prevBlock, BlockSize);
                        }
                        break;
                    }

                case EncryptionMode.OFB:
                    {
                        byte[] prevBlock = new byte[BlockSize];
                        byte[] curBlock = new byte[BlockSize];
                        byte[] encryptBlock = new byte[BlockSize];
                        Array.Copy(_initializationVector, prevBlock, prevBlock.Length);
                        for (int i = 0; i < res.Length / BlockSize; i++)
                        {
                            Array.Copy(res, i * BlockSize, curBlock, 0, BlockSize);
                            encryptBlock = algorithm.EncryptBlock(prevBlock);
                            blocks[i] = XOR(encryptBlock, curBlock);
                            Array.Copy(encryptBlock, prevBlock, BlockSize);
                        }
                        break;
                    }

                case EncryptionMode.CTR:
                    {
                        var copyInitializationVector = new Byte[BlockSize];
                        _initializationVector.CopyTo(copyInitializationVector, 0);
                        var blockList = GetListFromArrayWithBlockSizeLength(res);
                        var counterList = new List<Byte[]>();
                        for (var count = 0; count < res.Length / BlockSize; count++)
                        {
                            counterList.Add(copyInitializationVector);
                            IncrementCounterByOne(copyInitializationVector);
                        }

                        Parallel.For(0, res.Length / BlockSize, count =>

                            blocks[count] = XOR(algorithm.EncryptBlock(counterList[count]), blockList[count])
                        );
                        break;
                    }

                case EncryptionMode.RD:
                    {
                        var deltaArr = new Byte[BlockSize];
                        Array.Copy(_initializationVector, BlockSize, deltaArr, 0, BlockSize);
                        var copyInitializationVector = new Byte[BlockSize];
                        Array.Copy(_initializationVector, 0, copyInitializationVector, 0, BlockSize);
                        var initializationVector = BitConverter.ToUInt64(copyInitializationVector, 0);
                        blocks.Add(null);
                        blocks[0] = algorithm.EncryptBlock(copyInitializationVector);

                        var blockList = GetListFromArrayWithBlockSizeLength(res);
                        var counterList = new List<Byte[]>();
                        for (var count = 0; count < res.Length / BlockSize; count++)
                        {
                            counterList.Add(copyInitializationVector);
                            IncrementCounterByOne(copyInitializationVector);
                        }

                        Parallel.For(0, res.Length / BlockSize, count =>

                            blocks[count + 1] = algorithm.EncryptBlock(XOR(counterList[count], blockList[count]))
                        );
                        break;
                    }

                case EncryptionMode.RDH:
                    {
                        var deltaArr = new Byte[BlockSize];
                        Array.Copy(_initializationVector, BlockSize, deltaArr, 0, BlockSize);
                        var copyInitializationVector = new Byte[BlockSize];
                        Array.Copy(_initializationVector, 0, copyInitializationVector, 0, BlockSize);
                        blocks.Add(null);
                        blocks[0] = algorithm.EncryptBlock(copyInitializationVector);
                        blocks.Add(null);
                        var hashAlgoritm = SHA512.Create();
                        blocks[1] = XOR(copyInitializationVector, hashAlgoritm.ComputeHash(Encoding.Default.GetBytes(param)));

                        var blockList = GetListFromArrayWithBlockSizeLength(res);
                        var counterList = new List<Byte[]>();
                        for (var count = 0; count < res.Length / BlockSize; count++)
                        {
                            IncrementCounterByOne(copyInitializationVector);
                            counterList.Add(copyInitializationVector);
                        }

                        Parallel.For(0, res.Length / BlockSize, i =>

                            blocks[i + 2] = algorithm.EncryptBlock(XOR(counterList[i], blockList[i]))
                        );
                        break;
                    }
            }
            return MakeArrayFromList(blocks);
        }

        public byte[] DecryptBlock(byte[] data)
        {
            List<byte[]> blocks = Enumerable.Repeat(default(Byte[]), data.Length / BlockSize).ToList();

            switch (_encryptionMode)
            {
                case EncryptionMode.ECB:
                    {

                        List<byte[]> blockList = GetListFromArrayWithBlockSizeLength(data);

                        Parallel.For(0, data.Length / BlockSize, i =>

                            blocks[i] = algorithm.DecryptBlock(blockList[i])
                        );
                        break;
                    }

                case EncryptionMode.CBC:
                    {
                        List<byte[]> blockList = GetListFromArrayWithBlockSizeLength(data);
                        blockList.Insert(0, _initializationVector);

                        Parallel.For(0, data.Length / BlockSize, i =>

                            blocks[i] = XOR(blockList[i], algorithm.DecryptBlock(blockList[i + 1]))
                        );
                        break;
                        
                    }

                case EncryptionMode.CFB:
                    {
                        List<byte[]> blockList = GetListFromArrayWithBlockSizeLength(data);
                        blockList.Insert(0, _initializationVector);

                        Parallel.For(0, data.Length / BlockSize, i =>

                            blocks[i] = XOR(algorithm.EncryptBlock(blockList[i]), blockList[i + 1])
                        );
                        break;
                    }

                case EncryptionMode.OFB:
                    {
                        byte[] prevBlock = new byte[BlockSize];
                        byte[] curBlock = new byte[BlockSize];
                        byte[] encryptBlock = new byte[BlockSize];
                        Array.Copy(_initializationVector, prevBlock, prevBlock.Length);
                        for (int i = 0; i < data.Length / BlockSize; i++)
                        {
                            Array.Copy(data, i * BlockSize, curBlock, 0, BlockSize);
                            encryptBlock = algorithm.EncryptBlock(prevBlock);
                            blocks[i] = XOR(encryptBlock, curBlock);
                            Array.Copy(encryptBlock, prevBlock, BlockSize);
                        }
                        break;
                    }

                case EncryptionMode.CTR:
                    {
                        var copyInitializationVector = new Byte[BlockSize];
                        _initializationVector.CopyTo(copyInitializationVector, 0);
                        var blockList = GetListFromArrayWithBlockSizeLength(data);
                        var counterList = new List<Byte[]>();

                        for (var count = 0; count < data.Length / BlockSize; count++)
                        {
                            counterList.Add(copyInitializationVector);
                            IncrementCounterByOne(copyInitializationVector);
                        }

                        Parallel.For(0, data.Length / BlockSize, i =>

                            blocks[i] = XOR(algorithm.EncryptBlock(counterList[i]), blockList[i])
                        );
                        break;
                    }

                case EncryptionMode.RD:
                    {
                        var curBlock = new Byte[BlockSize];
                        var deltaArr = new Byte[BlockSize];
                        Array.Copy(_initializationVector, BlockSize, deltaArr, 0, BlockSize);
                        Array.Copy(data, 0, curBlock, 0, BlockSize);
                        var copyInitializationVector = algorithm.DecryptBlock(curBlock);
                        var blockList = GetListFromArrayWithBlockSizeLength(data);
                        var counterList = new List<Byte[]>();

                        for (var count = 0; count < data.Length / BlockSize; count++)
                        {
                            counterList.Add(copyInitializationVector);
                            IncrementCounterByOne(copyInitializationVector);
                        }

                        Parallel.For(1, data.Length / BlockSize, count =>

                            blocks[count - 1] = XOR(algorithm.DecryptBlock(blockList[count]), counterList[count - 1])
                        );
                        blocks.RemoveAt(blocks.Count - 1);
                        break;
                    }

                case EncryptionMode.RDH:
                    {
                        var curBlock = new Byte[BlockSize];
                        var deltaArr = new Byte[BlockSize];
                        Array.Copy(_initializationVector, BlockSize, deltaArr, 0, BlockSize);
                        Array.Copy(data, 0, curBlock, 0, BlockSize);
                        var copyInitializationVector = algorithm.DecryptBlock(curBlock);
                        var blockList = GetListFromArrayWithBlockSizeLength(data);
                        var hashAlgoritm = SHA512.Create();
                        blocks[1] = XOR(copyInitializationVector, hashAlgoritm.ComputeHash(Encoding.Default.GetBytes(param)));
                        if (!XOR(copyInitializationVector, hashAlgoritm.ComputeHash(Encoding.Default.GetBytes(param))).SequenceEqual(blockList[1]))
                        {
                            throw new ArgumentException(nameof(param));
                        }

                        var counterList = new List<Byte[]>();
                        for (var count = 0; count < data.Length / BlockSize - 2; count++)
                        {
                            IncrementCounterByOne(copyInitializationVector);
                            counterList.Add(copyInitializationVector);
                        }

                        Parallel.For(2, data.Length / BlockSize, i =>

                            blocks[i - 2] = XOR(algorithm.DecryptBlock(blockList[i]), counterList[i - 2])
                        );
                        blocks.RemoveAt(blocks.Count - 1);
                        blocks.RemoveAt(blocks.Count - 1);
                        break;
                    }
            }
            
            return padder.RemovePadding(blocks);
        }

        private Byte[] IncrementCounterByOne(Byte[] initCounterValue)
        {
            Byte[] tmp = (Byte[])initCounterValue.Clone();
            for (var i = BlockSize; i > 0; i--)
            {
                tmp[i - 1]++;
                if (tmp[i - 1] != 0)
                {
                    break;
                }
            }

            return tmp;
        }


        private byte[] MakeArrayFromList(List<byte[]> data)
        {
            byte[] res = new byte[BlockSize * data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                Array.Copy(data[i], 0, res, i * BlockSize, BlockSize);
            }
            return res;
        }

        private byte[] XOR(byte[] left, byte[] right)
        {
            byte[] res = new byte[left.Length];
            for (int i = 0; i < left.Length; i++)
            {
                res[i] = (byte)(left[i] ^ right[i]);
            }
            return res;
        }
    }
}
