using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;

namespace MCLiveStatus.Pinger.Schedulers
{
    public delegate Task StopPingSchedule();

    public interface IServerPingerScheduler
    {
        Task<StopPingSchedule> Schedule(ServerAddress serverAddress, double secondsInterval,
            Action<ServerPingResponse> onPing = null,
            Action<Exception> onException = null);
    }
}