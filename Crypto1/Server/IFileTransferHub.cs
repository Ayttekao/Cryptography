using System.Collections;
using CourseWork.AsymmetricAlgorithms.Benaloh.Algorithm;

namespace Server
{
    public interface IFileTransferHub
    {
        Task UnicastFilenames(ICollection filenames);

        Task AcceptFile(Byte[] file, String filename, String modeAsString, Byte[] iv);

        Task ReceivePublicKey(PublicKey publicKey);
    }
}