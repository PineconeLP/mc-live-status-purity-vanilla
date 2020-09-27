using System.Net.Sockets;
using System.Threading.Tasks;

namespace MCLiveStatus.Pinger.Models.NetworkStreams
{
    public class DefaultNetworkStream : INetworkStream
    {
        private readonly NetworkStream _networkStream;

        public DefaultNetworkStream(NetworkStream networkStream)
        {
            _networkStream = networkStream;
        }

        public Task<int> ReadAsync(byte[] data, int offset, int length)
        {
            return _networkStream.ReadAsync(data, offset, length);
        }

        public async Task WriteAsync(byte[] data)
        {
            await _networkStream.WriteAsync(data);
        }

        public void Dispose()
        {
            _networkStream.Dispose();
        }
    }
}