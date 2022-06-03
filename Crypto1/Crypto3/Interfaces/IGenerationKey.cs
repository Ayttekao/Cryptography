namespace Crypto3.Interfaces
{
    public interface IGenerationKey
    {
        byte[] GenerateRoundKeys(byte[] key);
    }
}
