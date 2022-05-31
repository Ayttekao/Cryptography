using System;
using System.IO;
using System.Threading.Tasks;

namespace Client.Stuff
{
    public class Utils
    {
        public static DirectoryInfo LoadStore(string absolutePath)
        {
            var directory = new DirectoryInfo(absolutePath);
            var directoryExists = directory.Exists;

            if (!directoryExists)
            {
                directory.Create();
                directory.Refresh();
            }

            return directory;
        }

        public static void RefreshStore(ref DirectoryInfo store)
        {
            store.Refresh();
        }
        
        public static void CreateDirectory(String folderName)
        {
            var currentPath = AppDomain.CurrentDomain.BaseDirectory + folderName;
            if (!Directory.Exists(currentPath))
            {
                Directory.CreateDirectory(currentPath);
            }
        }
        
        public static async Task<byte[]> ReadFileAsync(String path)
        {
            byte[] result;

            await using (var sourceStream = File.Open(path, FileMode.Open))
            {
                result = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(result, 0, (int) sourceStream.Length);
            }

            return result;
        }

        public static async Task WriteTextAsync(String filePath, Byte[] text)
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