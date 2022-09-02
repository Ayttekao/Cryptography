using System;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CourseWork.SymmetricAlgorithms.AlgorithmService.Modes;

namespace CipherStuffs;

public class Utils
{
    public static EncryptionMode ParseEncryptionMode(String modeAsString)
    {
        return (EncryptionMode)Enum.Parse(typeof(EncryptionMode), modeAsString);
    }
    
    public static BigInteger RandomBigInteger(BigInteger below, BigInteger above)
    {
        BigInteger randomBigInteger;
        var numberGenerator = RandomNumberGenerator.Create();
        var bytes = above.ToByteArray();

        do
        {
            numberGenerator.GetBytes(bytes);
            randomBigInteger = new BigInteger(bytes);
        } while (!(randomBigInteger >= below && randomBigInteger <= above));

        return randomBigInteger;
    }
    
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
            
    public static async Task<Byte[]> ReadFileAsync(String path)
    {
        Byte[] result;
    
        await using (var sourceStream = File.Open(path, FileMode.Open))
        {
            result = new Byte[sourceStream.Length];
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
            
    public static Byte[] GenerateIv(Int32 size)
    {
        return GetByteArray(size);
    }
            
    private static Byte[] GetByteArray(int size)
    {
        var rnd = new Random();
        var b = new byte[size];
        rnd.NextBytes(b);
        return b;
    }
}