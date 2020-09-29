using System.Net.Sockets;
using System.Threading.Tasks;

namespace MCLiveStatus.Pinger.NetworkStreams
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

        public async Task WriteAsync(byte[] data, int offset, int count)
        {
            await _networkStream.WriteAsync(data, offset, count);
        }

        public void Dispose()
        {
            _networkStream.Dispose();
        }
    }
}