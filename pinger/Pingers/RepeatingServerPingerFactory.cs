using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers.DinnerbonePython;
using MCLiveStatus.Pinger.Schedulers;

namespace MCLiveStatus.Pinger.Pingers
{
    public class RepeatingServerPingerFactory
    {
        private readonly IServerPingerScheduler _scheduler;

        public RepeatingServerPingerFactory()
        {
            IServerPinger pinger = new DinnerbonePythonServerPinger();

            _scheduler = new TimerServerPingerScheduler(pinger);
        }

        public RepeatingServerPinger CreateRepeatingServerPinger(ServerAddress serverAddress)
        {
            return new RepeatingServerPinger(serverAddress, _scheduler);
        }
    }
}