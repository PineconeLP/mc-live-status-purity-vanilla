using System.Threading.Tasks;
using System.Timers;

namespace MCLiveStatus.Pinger.Schedulers
{
    public class TimerServerPingerSchedulerHandler : IServerPingerSchedulerHandler
    {
        private readonly Timer _timer;

        public bool IsStopped { get; private set; }

        public TimerServerPingerSchedulerHandler(Timer timer)
        {
            _timer = timer;
        }

        public Task UpdatePingScheduleInterval(double intervalInSeconds)
        {
            _timer.Interval = intervalInSeconds * 1000;

            return Task.CompletedTask;
        }

        public Task StopPingSchedule()
        {
            _timer.Dispose();
            IsStopped = true;

            return Task.CompletedTask;
        }


    }
}