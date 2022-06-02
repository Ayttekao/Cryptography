using System.Collections.Concurrent;
using System.Numerics;
using CipherStuffs;
using CipherStuffs.Handshake;
using CourseWork.Benaloh.ProbabilisticSimplicityTest;
using CourseWork.LOKI97.Algorithm.BlockPacker;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;
using CourseWork.LOKI97.Algorithm.EncryptionTransformation;
using CourseWork.LOKI97.Algorithm.KeyGen;
using CourseWork.LOKI97.AlgorithmService.Modes;
using Microsoft.AspNetCore.SignalR;

namespace Server
{
    public class FileTransferHub : Hub<IFileTransferHub>
    {
        private static readonly String CurrentPath = AppDomain.CurrentDomain.BaseDirectory + FileFolderName;
        private static ConcurrentDictionary<String, Byte[]> _sessionKeys = new();
        private static ConcurrentBag<String> _fileNames = new();
        private static Boolean _isFileDirectoryExist = false;
        private static Handshaker _handshaker = new(TestType.MillerRabin, 0.7, 16, 2, 16);
        private const String FileFolderName = "Downloads";
        private CipherService _cipherService;

        public async Task SendPublicKey()
        {
            await Clients.Caller.ReceivePublicKey(_handshaker.GetPublicKey());
            Console.WriteLine("Send public key: {0}", String.Join(", ", _handshaker.GetPublicKey()));
        }

        public async Task AcceptSessionKey(List<BigInteger> encryptedSessionKey, String connectionId)
        {
            var sessionKey = _handshaker.DecryptSessionKey(encryptedSessionKey);
            Console.WriteLine("Accepted session key {0}", String.Join(", ", sessionKey));
            _sessionKeys.TryAdd(connectionId, sessionKey);
        }
        
        public async Task ScanFilesDir()
        {
            CheckingExistenceDirectory();
            var files = new DirectoryInfo(CurrentPath).GetFiles().Select(o => o.Name);
            _fileNames = new ConcurrentBag<String>(files);
            await Clients.Caller.UnicastFilenames(_fileNames);
        }

        public async Task BroadcastFile(String fileName, Byte[] file, Byte[] iv, String connectionId, String modeAsString)
        {
            var mode = (EncryptionMode)Enum.Parse(typeof(EncryptionMode), modeAsString);
            var sessionKey = _sessionKeys.First(x => x.Key == connectionId).Value;
            Console.WriteLine("Session key is {0}", String.Join(", ", sessionKey));
            Console.WriteLine("IV is {0}", String.Join(", ", iv));
            var algorithm = new Loki97Impl(new Encryption(), new BlockPacker(), new KeyGen(), sessionKey);
            _cipherService = new CipherService(algorithm, iv);
            
            CheckingExistenceDirectory();
            if (!_fileNames.Contains(fileName))
            {
                var fullPath = CurrentPath + "\\" + fileName;
                var decrypt = _cipherService.Decrypt(file, mode);
                await WriteTextAsync(fullPath, decrypt);
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
        
        private static void CheckingExistenceDirectory()
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
        
        private static async Task<Byte[]> ReadFileAsync(String path)
        {
            Byte[] result;
            // filestream

            await using (var sourceStream = File.Open(path, FileMode.Open))
            {
                result = new Byte[sourceStream.Length];
                await sourceStream.ReadAsync(result, 0, (int)sourceStream.Length);
            }

            return result;
        }
    }
}