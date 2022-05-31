using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Client.Stuff;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.SignalRClient
{
    public class SignalRClientImpl : SignalRClientBase
    {
        private const String DownloadFolderName = "Downloads";
        private static readonly string CurrentPath = AppDomain.CurrentDomain.BaseDirectory + DownloadFolderName;
        private DirectoryInfo _localStore = Utils.LoadStore(CurrentPath);
        private ICollection<String> _serverStore;

        public Task RegistersHandlers()
        {
            OnUnicastFilenames();
            OnAcceptFile();
            return Task.CompletedTask;
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

        public async Task SendFile(String filePath)
        {
            await HubConnection.InvokeAsync(nameof(SendFile), filePath);
        }

        public async Task BroadcastFile(String nameFile, String modeAsString)
        {
            var file = Utils.ReadFileAsync(nameFile).Result;
            //шифруем и отправляем айдишник
            await HubConnection.InvokeAsync(nameof(BroadcastFile),
                Path.GetFileName(nameFile),
                file,
                HubConnection.ConnectionId,
                modeAsString);
        }
        
        public SignalRClientImpl(string hubPath) : base(hubPath) { }

        public async Task AcceptFile(Byte[] file, string filename)
        {
            if (_localStore == null || _localStore.GetFiles().All(x => x.Name != filename))
            {
                var fullPath = CurrentPath + "\\" + filename;
                //расшифровать
                await Utils.WriteTextAsync(fullPath, file);
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
                HubConnection.On<Byte[], string>("AcceptFile", async (file, filename) =>
                {
                    await AcceptFile(file, filename);
                });
            }
            
            return Task.CompletedTask;
        }
    }
}