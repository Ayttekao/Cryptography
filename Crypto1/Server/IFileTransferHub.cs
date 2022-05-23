using System.Collections;
using System.Threading.Tasks;

namespace Server
{
    public interface IFileTransferHub
    {
        Task UnicastNewFiles(ICollection filenames);
    }
}