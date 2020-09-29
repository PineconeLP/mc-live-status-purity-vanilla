using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;

namespace MCLiveStatus.Pinger.Schedulers
{
    public interface IServerPingerScheduler
    {
        Task<Func<Task>> Schedule(ServerAddress serverAddress, int secondsInterval, Action<ServerPingResponse> onPing = null);
    }
}