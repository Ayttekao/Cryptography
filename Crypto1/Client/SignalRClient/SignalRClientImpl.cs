using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CipherStuffs;
using CipherStuffs.Handshake;
using CourseWork.AsymmetricAlgorithms.Benaloh.Algorithm;
using CourseWork.AsymmetricAlgorithms.Benaloh.ProbabilisticSimplicityTest;
using CourseWork.SymmetricAlgorithms.AlgorithmService.Modes;
using CourseWork.SymmetricAlgorithms.LOKI97.Algorithm;
using CourseWork.SymmetricAlgorithms.LOKI97.Algorithm.BlockPacker;
using CourseWork.SymmetricAlgorithms.LOKI97.Algorithm.EncryptionTransformation;
using CourseWork.SymmetricAlgorithms.LOKI97.Algorithm.KeyGen;
using CourseWork.SymmetricAlgorithms.TwoFish.Algorithm;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.SignalRClient
{
    public sealed class SignalRClientImpl : SignalRClientBase
    {
        private const Int32 KeySize = 32;
        private static readonly string CurrentPath = AppDomain.CurrentDomain.BaseDirectory + DownloadFolderName;
        private const String DownloadFolderName = "Downloads";
        private DirectoryInfo _localStore = Utils.LoadStore(CurrentPath);
        private ICollection<String> _serverStore;
        private Handshaker _handshaker = new(TestType.MillerRabin, 0.7, 16, 2, 16);
        private Byte[] _sessionKey;
        private CipherService _cipherService;

        public SignalRClientImpl(string hubPath) : base(hubPath) { }
        
        public Task RegistersHandlers()
        {
            OnUnicastFilenames();
            OnAcceptFile();
            OnReceivePublicKey();
            return Task.CompletedTask;
        }

        public async Task ReceivePublicKey(PublicKey publicKey)
        {
            _handshaker.SetPublicKey(publicKey);
        }
        
        public ICollection<String> GetServerStore()
        {
            return _serverStore;
        }
        
        public Task UnicastFilenames(ICollection<string> fileNames)
        {
            _serverStore = fileNames;
            return Task.CompletedTask;
        }

        public async Task ScanFilesDir()
        {
            await HubConnection.InvokeAsync(nameof(ScanFilesDir));
        }

        public async Task Handshake()
        {
            await RequestPublicKey();
            _sessionKey = _handshaker.GenerateSessionKey(KeySize);
            var encryptSessionKey = _handshaker.EncryptSessionKey(_sessionKey);
            await SendSessionKey(encryptSessionKey, HubConnection.ConnectionId);
        }

        public async Task RequestPublicKey()
        {
            await HubConnection.InvokeAsync("SendPublicKey");
        }

        public async Task SendSessionKey(List<BigInteger> encryptedSessionKey, String connectionId)
        {
            await HubConnection.InvokeAsync("AcceptSessionKey", encryptedSessionKey, connectionId);
        }
        public async Task SendFile(String filePath, String modeAsString)
        {
            await HubConnection.InvokeAsync(nameof(SendFile), filePath, modeAsString, HubConnection.ConnectionId);
        }

        public async Task BroadcastFile(String nameFile, String modeAsString)
        {
            var mode = Utils.ParseEncryptionMode(modeAsString);
            var algorithm = new TwoFishImpl(_sessionKey);//new Loki97Impl(new Encryption(), new BlockPacker(), new KeyGen(), _sessionKey);
            var iv = mode is EncryptionMode.RD or EncryptionMode.RDH 
                ? Utils.GenerateIv(algorithm.GetBlockSize() * 2)
                : Utils.GenerateIv(algorithm.GetBlockSize());
            _cipherService = new CipherService(algorithm, iv);

            var file = await _cipherService.Encrypt(nameFile, mode);
            await HubConnection.InvokeAsync(nameof(BroadcastFile),
                Path.GetFileName(nameFile),
                file,
                iv,
                HubConnection.ConnectionId,
                modeAsString);
        }

        public async Task AcceptFile(Byte[] file, String filename, String modeAsString, Byte[] iv)
        {
            var mode = Utils.ParseEncryptionMode(modeAsString);
            var algorithm = new TwoFishImpl(_sessionKey);//new Loki97Impl(new Encryption(), new BlockPacker(), new KeyGen(), _sessionKey);
            _cipherService = new CipherService(algorithm, iv);
            
            if (_localStore == null || _localStore.GetFiles().All(x => x.Name != filename))
            {
                Utils.RefreshStore(ref _localStore);
                var fullPath = CurrentPath + "\\" + filename;
                await _cipherService.Decrypt(fullPath, file, mode);
                Utils.RefreshStore(ref _localStore);
            }
        }

        private Task OnUnicastFilenames()
        {
            if (!Started)
            {
                HubConnection.On<ICollection<String>>("UnicastFilenames", async fileNames =>
                {
                    await UnicastFilenames(fileNames);
                });
            }
            
            return Task.CompletedTask;
        }

        private Task OnAcceptFile()
        {
            if (!Started)
            {
                HubConnection.On<Byte[], String, String, Byte[]>("AcceptFile", async (file, filename, modeAsString, iv) =>
                {
                    await AcceptFile(file, filename, modeAsString, iv);
                });
            }
            
            return Task.CompletedTask;
        }

        private Task OnReceivePublicKey()
        {
            if (!Started)
            {
                HubConnection.On<PublicKey>("ReceivePublicKey", async publicKey =>
                {
                    await ReceivePublicKey(publicKey);
                });
            }
            
            return Task.CompletedTask;
        }
    }
}