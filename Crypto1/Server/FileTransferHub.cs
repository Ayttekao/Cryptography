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
        private CipherService _cipherService;
        private DirectoryInfo _localStore = Utils.LoadStore(CurrentPath);
        private const String FileFolderName = "Downloads";
        private static ConcurrentDictionary<String, Byte[]> _sessionKeys = new();
        private static Handshaker _handshaker = new(TestType.MillerRabin, 0.7, 16, 2, 16);
        private static readonly String CurrentPath = AppDomain.CurrentDomain.BaseDirectory + FileFolderName;

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
            _localStore = Utils.LoadStore(CurrentPath);
            await Clients.Caller.UnicastFilenames(_localStore.GetFiles().Select(x => x.Name).ToList());
        }

        public async Task BroadcastFile(String fileName, Byte[] file, Byte[] iv, String connectionId, String modeAsString)
        {
            var mode = (EncryptionMode)Enum.Parse(typeof(EncryptionMode), modeAsString);
            var sessionKey = _sessionKeys.First(x => x.Key == connectionId).Value;
            var algorithm = new Loki97Impl(new Encryption(), new BlockPacker(), new KeyGen(), sessionKey);
            _cipherService = new CipherService(algorithm, iv);
            
            if (_localStore == null || _localStore.GetFiles().All(x => x.Name != fileName))
            {
                var fullPath = CurrentPath + "\\" + fileName;
                await _cipherService.Decrypt(fullPath, file, mode);
                Utils.RefreshStore(ref _localStore);
                await Clients.All.UnicastFilenames(_localStore.GetFiles().Select(x => x.Name).ToList());
            }
        }

        public async Task SendFile(string fileName, String modeAsString)
        {
            var fullPath = CurrentPath + "\\" + fileName;
            var file = await ReadFileAsync(fullPath);
            //зашифровать
            await Clients.Caller.AcceptFile(file, fileName, modeAsString);
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