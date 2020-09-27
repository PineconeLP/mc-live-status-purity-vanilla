using System;
using System.Text;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models.NetworkStreams;
using MCLiveStatus.Pinger.Models.TcpClients;

namespace MCLiveStatus.Pinger.Models.ServerStatusClients
{
    public class ServerStatusClient : IDisposable
    {
        private readonly ITcpClient _client;
        private INetworkStream _stream;

        public bool IsConnected { get; private set; }

        public ServerStatusClient(ITcpClient client)
        {
            _client = client;
        }

        public async Task Connect(string host, int port)
        {
            await _client.ConnectAsync(host, port);
            _stream = _client.GetStream();

            IsConnected = true;
        }

        public void Disconnect()
        {
            _stream.Dispose();
            _client.Close();
        }

        public async Task SendPing()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("Client not connected.");
            }

            await _stream.WriteAsync(new byte[] { 0xFE, 0x01 });
        }

        public async Task<ServerStatus> ReadPingResponse()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("Client not connected.");
            }

            byte[] data = new byte[255];
            int length = await _stream.ReadAsync(data, 0, data.Length);

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

        public void Dispose()
        {
            _stream.Dispose();
            _client.Dispose();
        }
    }
}