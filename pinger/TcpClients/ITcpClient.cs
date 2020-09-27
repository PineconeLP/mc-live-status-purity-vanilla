using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.NetworkStreams;

namespace MCLiveStatus.Pinger.TcpClients
{
    public delegate ITcpClient CreateTcpClient();

    public interface ITcpClient : IDisposable
    {
        Task ConnectAsync(string host, int port);

        INetworkStream GetStream();
    }
}