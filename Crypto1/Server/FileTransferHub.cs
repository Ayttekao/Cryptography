using System.Collections.Concurrent;
using System.Numerics;
using CourseWork.Benalo.Algorithm;
using CourseWork.Benalo.ProbabilisticSimplicityTest;
using Microsoft.AspNetCore.SignalR;

namespace Server
{
    public class FileTransferHub : Hub<IFileTransferHub>
    {
        private static ConcurrentBag<String> _fileNames = new();
        private const String FileFolderName = "Downloads";
        private const Int32 ValuesCount = 4;
        private const Int32 BlockSize = 8;
        private static readonly String CurrentPath = AppDomain.CurrentDomain.BaseDirectory + FileFolderName;
        private static Boolean _isFileDirectoryExist = false;
        private static Benaloh benalo = new(TestType.MillerRabin, 0.7, 16, BigInteger.Pow(2, 16) + BigInteger.One);
        private static ConcurrentDictionary<String, BigInteger> _privateKeys = new();
        private static Byte[] iv;
        //public PublicKeys testKey;
        
        public async Task ScanFilesDir()
        {
            CheckingExistenceDirectory();
            var files = new DirectoryInfo(CurrentPath).GetFiles().Select(o => o.Name);
            _fileNames = new ConcurrentBag<String>(files);
            await Clients.Caller.UnicastFilenames(_fileNames);
        }

        public async Task ReceiveEncryptPrivateKey(String connectionId, List<BigInteger> encryptPrivateKey)
        {
            var keyByteArray = new byte[ValuesCount][];
            
            for (var i = 0; i < ValuesCount; i++)
            {
                var message = benalo.Decrypt(encryptPrivateKey[i]);
                keyByteArray[i] = message.ToByteArray();
            }
            var temp = new byte[BlockSize];
            var count = 0;
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    temp[count] = keyByteArray[i][j];
                    count++;
                }
            }
            var privateKey = new BigInteger(temp);

            Console.WriteLine("Accept private key: {0}", privateKey);
            _privateKeys.TryAdd(connectionId, privateKey);
        }

        public async Task UnicastPublicKey()
        {
            var keys = benalo.GetPublicKey();
            //testKey = keys;
            var random = new Random();
            var buffer = new byte[BlockSize];
            random.NextBytes(buffer);
            buffer[^1] = 0b00000000;
            iv = buffer;
            var localIv = new BigInteger(buffer);
            
            List<BigInteger> publicKeysAndIV = new List<BigInteger>() { keys.n, keys.r, keys.y, localIv };
            await Clients.Caller.RecivePublicKey(publicKeysAndIV);
        }

        public async Task BroadcastFile(
            String fileName, 
            Byte[] file, 
            String connectionId, 
            String modeAsString)
        {
            CheckingExistenceDirectory();
            if (!_fileNames.Contains(fileName))
            {
                //var mode = (EncryptionMode)Enum.Parse(typeof(EncryptionMode), modeAsString!);
                //var key = _privateKeys.First(x => x.Key.Equals(connectionId)).Value.ToByteArray();
                //var encryptedFIle = AlgorithmService(file, key, iv, mode, false);
                var fullPath = CurrentPath + "\\" + fileName;
                await WriteTextAsync(fullPath, file);
                _fileNames.Add(fileName);
                await Clients.Caller.UnicastFilenames(_fileNames);
            }
        }

        public async Task SendFile(string fileName)
        {
            var fullPath = CurrentPath + "\\" + fileName;
            var file = await ReadFileAsync(fullPath);
            //зашифровать
            await Clients.Caller.AcceptFile(file, fileName);
        }
        private void CheckingExistenceDirectory()
        {
            if (!_isFileDirectoryExist)
            {
                Directory.CreateDirectory(CurrentPath);
            }
        }

        private static async Task WriteTextAsync(String filePath, Byte[] text)
        {
            using var sourceStream =
                new FileStream(
                    filePath,
                    FileMode.Create, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true);
            //расшифровать 
            await sourceStream.WriteAsync(text, 0, text.Length);
        }
        
        private static async Task<byte[]> ReadFileAsync(String path)
        {
            byte[] result;

            await using (var sourceStream = File.Open(path, FileMode.Open))
            {
                result = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(result, 0, (int)sourceStream.Length);
            }

            return result;
        }
        
        /*private byte[] AlgorithmService(byte[] inputBuffer, byte[] privateKey, byte[] iv, EncryptionMode encryptionMode, bool toEncrypt)
        {
            var safer = new Safer64_K(new GeneratingKeys());
            var encryptor = new Modes(encryptionMode, iv)
            {
                algorithm = safer
            };
            safer.SetKey(privateKey);
            
            return toEncrypt ? encryptor.BlockEncrypt(inputBuffer) : encryptor.BlockDecrypt(inputBuffer);
        }*/
    }
}