namespace CourseWork.SymmetricAlgorithms.CipherAlgorithm.Magenta.Algorithm;

public class MagentaImpl : ICipherAlgorithm
{
    private readonly byte[] _key;
    private const int BlockSize = 16;

    public MagentaImpl(byte[] key)
    {
        _key = key;
    }
    public byte[] BlockEncrypt(byte[] input, int inOffset)
    {
        return Functions.Encrypt(input, _key);
    }

    public byte[] BlockDecrypt(byte[] input, int inOffset)
    {
        return Functions.Decrypt(input, _key);
    }

    public int GetBlockSize()
    {
        return BlockSize;
    }
}