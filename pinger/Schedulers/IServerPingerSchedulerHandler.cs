using System.Threading.Tasks;

namespace MCLiveStatus.Pinger.Schedulers
{
    public interface IServerPingerSchedulerHandler
    {
        bool IsStopped { get; }

        /// <summary>
        /// Update the schedulers ping interval.
        /// </summary>
        /// <param name="intervalInSeconds">The updated interval in seconds.</param>
        /// <exception cref="ArgumentException">Thrown if interval not greater than 0.</exception>
        Task UpdatePingScheduleInterval(double intervalInSeconds);

        Task StopPingSchedule();
    }
}