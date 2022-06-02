using System;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.FileProcessing;
using CourseWork.LOKI97.Algorithm.CipherAlgorithm;
using CourseWork.LOKI97.AlgorithmService.Modes;

namespace CipherStuffs;

public class CipherService
{
    private ICipherAlgorithm _algorithm;
    private ParallelCipher _parallelCipher;
    private Byte[] _iv;

    public CipherService(ICipherAlgorithm algorithm, Byte[] iv)
    {
        _algorithm = algorithm;
        _iv = iv.ToArray();
        _parallelCipher = new ParallelCipher(algorithm, iv.ToArray());
    }

    public async Task<Byte[]> Encrypt(String path, EncryptionMode mode)
    {
        return await _parallelCipher.Encrypt(path, mode);
    }

    public async Task Decrypt(String path, Byte[] inputBuffer, EncryptionMode mode)
    {
        await _parallelCipher.Decrypt(path, inputBuffer, mode);
    }

    public Byte[] Decrypt(Byte[] inputBuffer, EncryptionMode mode)
    {
        return _parallelCipher.Decrypt(inputBuffer, mode).Result;
    }

    public Byte[] GetIv()
    {
        return _iv;
    }

    public void SetIv(Byte[] iv)
    {
        _iv = iv;
    }
    
    public Byte[] GenerateIv(EncryptionMode mode)
    {
        return mode is EncryptionMode.RD or EncryptionMode.RDH 
            ? GetByteArray(_algorithm.GetBlockSize() * 2)
            : GetByteArray(_algorithm.GetBlockSize());
    }

    private Byte[] GetByteArray(int size)
    {
        var rnd = new Random();
        var b = new byte[size];
        rnd.NextBytes(b);
        return b;
    }
}