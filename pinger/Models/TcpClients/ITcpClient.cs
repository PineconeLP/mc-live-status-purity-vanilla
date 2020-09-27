using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models.NetworkStreams;

namespace MCLiveStatus.Pinger.Models.TcpClients
{
    public delegate ITcpClient CreateTcpClient();

    public interface ITcpClient : IDisposable
    {
        Task ConnectAsync(string host, int port);

        INetworkStream GetStream();
    }
}