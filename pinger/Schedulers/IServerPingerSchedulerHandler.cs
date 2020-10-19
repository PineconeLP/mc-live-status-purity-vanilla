using System.Threading.Tasks;

namespace MCLiveStatus.Pinger.Schedulers
{
    public interface IServerPingerSchedulerHandler
    {
        bool IsStopped { get; }

        Task UpdatePingScheduleInterval(double intervalInSeconds);

        Task StopPingSchedule();
    }
}