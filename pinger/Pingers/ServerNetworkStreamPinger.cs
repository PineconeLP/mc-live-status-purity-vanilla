using System;
using System.Text;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.NetworkStreams;

namespace MCLiveStatus.Pinger.Services
{
    public class ServerNetworkStreamPinger
    {
        public async Task<ServerStatus> Ping(INetworkStream stream)
        {
            await SendPing(stream);

            return await ReadPing(stream);
        }

        private async Task SendPing(INetworkStream stream)
        {
            await stream.WriteAsync(new byte[] { 0xFE, 0x01 });
        }

        private async Task<ServerStatus> ReadPing(INetworkStream stream)
        {
            byte[] data = new byte[255];
            int length = await stream.ReadAsync(data, 0, data.Length);

            string response = Encoding.BigEndianUnicode.GetString(data, 3, length - 3);

            return ConvertPingResponseToServerStatus(response);
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