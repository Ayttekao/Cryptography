namespace Crypto3.Interfaces
{
    public interface IChipher
    {
        byte[] EncryptBlock(byte[] message);
        byte[] DecryptBlock(byte[] message);
        void SetKey(byte[] key);
    }
}
