using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;

namespace MCLiveStatus.Pinger.Schedulers
{
    public interface IServerPingerScheduler
    {
        Task<IServerPingerSchedulerHandler> Schedule(ServerAddress serverAddress, double secondsInterval,
            Action<ServerPingResponse> onPing = null,
            Action<Exception> onException = null);
    }
}