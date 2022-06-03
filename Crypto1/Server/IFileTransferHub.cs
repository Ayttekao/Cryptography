using System.Collections;
using CourseWork.Benaloh.Algorithm;

namespace Server
{
    public interface IFileTransferHub
    {
        Task UnicastFilenames(ICollection filenames);

        Task AcceptFile(Byte[] file, String filename, String modeAsString, Byte[] iv);

        Task ReceivePublicKey(PublicKey publicKey);
    }
}