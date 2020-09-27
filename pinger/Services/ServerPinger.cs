using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Models.NetworkStreams;
using MCLiveStatus.Pinger.Models.TcpClients;

namespace MCLiveStatus.Pinger.Services
{
    public class ServerPinger
    {
        private readonly CreateTcpClient _createTcpClient;

        public ServerPinger(CreateTcpClient createTcpClient)
        {
            _createTcpClient = createTcpClient;
        }

        public async Task<ServerStatus> Ping(string host, int port)
        {
            using (ITcpClient client = _createTcpClient())
            {
                await client.ConnectAsync(host, port);

                using (INetworkStream stream = client.GetStream())
                {
                    await SendPing(stream);

                    string response = await ReadPingResponse(stream);

                    return ConvertPingResponseToServerStatus(response);
                }
            }
        }

        private async Task SendPing(INetworkStream stream)
        {
            await stream.WriteAsync(new byte[] { 0xFE, 0x01 });
        }

        private async Task<string> ReadPingResponse(INetworkStream stream)
        {
            byte[] data = new byte[255];
            int length = await stream.ReadAsync(data, 0, data.Length);

            return Encoding.BigEndianUnicode.GetString(data, 3, length - 3);
        }

        private ServerStatus ConvertPingResponseToServerStatus(string pingResponse)
        {
            string[] splitResponse = pingResponse.Split('\u0000');

            int onlinePlayers = 0;
            if (splitResponse.Length >= 3)
            {
                int.TryParse(splitResponse[4], out onlinePlayers);
            }

            int maxPlayers = 0;
            if (splitResponse.Length >= 4)
            {
                int.TryParse(splitResponse[5], out maxPlayers);
            }

            return new ServerStatus()
            {
                OnlinePlayers = onlinePlayers,
                MaxPlayers = maxPlayers
            };
        }
    }
}