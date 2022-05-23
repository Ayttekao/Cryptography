using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Server
{
    public class FileTransferHub : Hub<IFileTransferHub>
    {
        private static ConcurrentBag<String> _fileNames = new();
        private const String FileFolderName = "Downloads";
        private static readonly String CurrentPath = AppDomain.CurrentDomain.BaseDirectory + FileFolderName;
        private static Boolean _isFileDirectoryExist = false;

        public async Task ScanFilesDir()
        {
            CheckingExistenceDirectory();
            var files = new DirectoryInfo(CurrentPath).GetFiles().Select(o => o.Name);
            _fileNames = new ConcurrentBag<String>(files);
            //await Clients.Caller.SendAsync("UnicastNewFiles", _fileNames);
            await Clients.Caller.UnicastNewFiles(_fileNames);
        }

        public async Task BroadcastFile(String fileName, Byte[] file)
        {
            CheckingExistenceDirectory();
            if (!_fileNames.Contains(fileName))
            {
                var fullPath = CurrentPath + "\\" + fileName;
                await WriteTextAsync(fullPath, file);
                _fileNames.Add(fileName);
                //await Clients.Caller.SendAsync("UnicastNewFiles", _fileNames);
                await Clients.Caller.UnicastNewFiles(_fileNames);
            }
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

            await sourceStream.WriteAsync(text, 0, text.Length);
        }
    }
}