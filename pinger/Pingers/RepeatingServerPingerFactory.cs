using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers.DinnerbonePython;
using MCLiveStatus.Pinger.Schedulers;
using MCLiveStatus.Pinger.Schedulers.Quartz;
using Quartz.Impl;

namespace MCLiveStatus.Pinger.Pingers
{
    public class RepeatingServerPingerFactory
    {
        private readonly IServerPingerScheduler _scheduler;

        public RepeatingServerPingerFactory()
        {
            IServerPinger pinger = new DinnerbonePythonServerPinger();

            _scheduler = new QuartzServerPingerScheduler(new StdSchedulerFactory(), pinger);
        }

        public RepeatingServerPinger CreateRepeatingServerPinger(ServerAddress serverAddress)
        {
            return new RepeatingServerPinger(serverAddress, _scheduler);
        }
    }
}