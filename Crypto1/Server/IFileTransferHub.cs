using System.Collections;
using System.Numerics;

namespace Server
{
    public interface IFileTransferHub
    {
        Task BroadCastPublicKey(Byte[] key);
        Task UnicastFilenames(ICollection filenames);

        Task AcceptFile(Byte[] file, string filename);

        Task RecivePublicKey(List<BigInteger> publicKeys);
    }
}