using System.Collections;
using CourseWork.Benaloh.Algorithm;

namespace Server
{
    public interface IFileTransferHub
    {
        Task BroadCastPublicKey(Byte[] key);
        Task UnicastFilenames(ICollection filenames);

        Task AcceptFile(Byte[] file, string filename);

        Task ReceivePublicKey(PublicKey publicKey);

        Task SendSessionKey();
    }
}