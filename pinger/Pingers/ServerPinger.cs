using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.NetworkStreams;
using MCLiveStatus.Pinger.TcpClients;

namespace MCLiveStatus.Pinger.Pingers
{
    public class ServerPinger
    {
        private readonly CreateTcpClient _createClient;
        private readonly ServerNetworkStreamPinger _streamPinger;

        public ServerPinger(CreateTcpClient createClient, ServerNetworkStreamPinger streamPinger)
        {
            _createClient = createClient;
            _streamPinger = streamPinger;
        }

        public async Task<ServerPingResponse> Ping(ServerAddress address)
        {
            return await Ping(address.Host, address.Port);
        }

        public async Task<ServerPingResponse> Ping(string host, int port)
        {
            using (ITcpClient client = _createClient())
            {
                await client.ConnectAsync(host, port);

                using (INetworkStream stream = client.GetStream())
                {
                    return await _streamPinger.Ping(stream);
                }
            }
        }
    }
}