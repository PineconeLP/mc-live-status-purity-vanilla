using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Models.NetworkStreams;
using MCLiveStatus.Pinger.Models.ServerStatusClients;
using MCLiveStatus.Pinger.Models.TcpClients;

namespace MCLiveStatus.Pinger.Services
{
    public class ServerPinger
    {
        private readonly ServerStatusClientFactory _clientFactory;

        public ServerPinger(ServerStatusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ServerStatus> Ping(string host, int port)
        {
            using (ServerStatusClient client = _clientFactory.CreateClient())
            {
                await client.Connect(host, port);
                await client.SendPing();

                return await client.ReadPingResponse();
            }
        }
    }
}