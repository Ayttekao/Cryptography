using System.Threading.Tasks;

namespace Client.SignalRClient
{
    public interface ISignalRClient
    {
        bool IsConnected { get; }
        Task Start();
    }
}