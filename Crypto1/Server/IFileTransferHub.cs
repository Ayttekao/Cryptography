using System.Collections;
using System.Numerics;

namespace Server
{
    public interface IFileTransferHub
    {
        Task BroadCastPublicKey(byte[] key);
        Task UnicastFilenames(ICollection filenames);

        Task AcceptFile(byte[] file, string filename);

        Task RecivePublicKey(List<BigInteger> publicKeys);
    }
}