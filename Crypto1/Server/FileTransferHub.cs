using System.Collections.Concurrent;
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
            await Clients.Caller.UnicastFilenames(_fileNames);
        }

        public async Task BroadcastFile(String fileName, Byte[] file, String connectionId, String modeAsString)
        {
            CheckingExistenceDirectory();
            if (!_fileNames.Contains(fileName))
            {
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