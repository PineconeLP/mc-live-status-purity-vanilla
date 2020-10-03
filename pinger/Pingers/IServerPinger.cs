using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;

namespace MCLiveStatus.Pinger.Pingers
{
    public interface IServerPinger
    {
        Task<ServerPingResponse> Ping(ServerAddress address);
        Task<ServerPingResponse> Ping(string host, int port = 0);
    }
}