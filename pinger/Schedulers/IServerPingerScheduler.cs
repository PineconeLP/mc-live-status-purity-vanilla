using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;

namespace MCLiveStatus.Pinger.Schedulers
{
    public interface IServerPingerScheduler
    {
        Task<Func<Task>> Start(ServerAddress serverAddress, Action<ServerPingResponse> onPing = null);
    }
}