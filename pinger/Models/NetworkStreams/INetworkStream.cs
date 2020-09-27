using System;
using System.Threading.Tasks;

namespace MCLiveStatus.Pinger.Models.NetworkStreams
{
    public interface INetworkStream : IDisposable
    {
        Task<int> ReadAsync(byte[] data, int offset, int length);
        Task WriteAsync(byte[] data);
    }
}