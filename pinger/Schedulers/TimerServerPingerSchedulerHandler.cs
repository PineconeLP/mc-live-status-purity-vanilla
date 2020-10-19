using System;
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

        /// <summary>
        /// Update the schedulers ping interval.
        /// </summary>
        /// <param name="intervalInSeconds">The updated interval in seconds.</param>
        /// <exception cref="ArgumentException">Thrown if interval not greater than 0.</exception>
        public Task UpdatePingScheduleInterval(double intervalInSeconds)
        {
            if (intervalInSeconds <= 0)
            {
                throw new ArgumentException("Interval in seconds must be greater than 0.", nameof(intervalInSeconds));
            }

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