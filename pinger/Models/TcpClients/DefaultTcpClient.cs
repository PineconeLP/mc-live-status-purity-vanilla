using System.Net.Sockets;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models.NetworkStreams;

namespace MCLiveStatus.Pinger.Models.TcpClients
{
    public class DefaultTcpClient : ITcpClient
    {
        private readonly TcpClient _client;

        public DefaultTcpClient()
        {
            _client = new TcpClient();
        }

        public Task ConnectAsync(string host, int port)
        {
            return _client.ConnectAsync(host, port);
        }

        public void Close()
        {
            _client.Close();
        }

        public INetworkStream GetStream()
        {
            return new DefaultNetworkStream(_client.GetStream());
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}