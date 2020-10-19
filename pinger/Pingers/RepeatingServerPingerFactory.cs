using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Schedulers;

namespace MCLiveStatus.Pinger.Pingers
{
    public class RepeatingServerPingerFactory
    {
        private readonly IServerPingerScheduler _scheduler;

        public RepeatingServerPingerFactory(IServerPingerScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public RepeatingServerPinger CreateRepeatingServerPinger(ServerAddress serverAddress)
        {
            return new RepeatingServerPinger(serverAddress, _scheduler);
        }
    }
}