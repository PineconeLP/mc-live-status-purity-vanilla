using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Schedulers;
using MCLiveStatus.Pinger.Schedulers.Quartz;
using MCLiveStatus.Pinger.TcpClients;
using Quartz.Impl;

namespace MCLiveStatus.Pinger.Pingers
{
    public class RepeatingServerPingerFactory
    {
        private readonly IServerPingerScheduler _scheduler;

        public RepeatingServerPingerFactory()
        {
            CreateTcpClient createClient = () => new DefaultTcpClient();
            ServerNetworkStreamPinger streamPinger = new ServerNetworkStreamPinger();
            ServerPinger pinger = new ServerPinger(createClient, streamPinger);

            _scheduler = new QuartzServerPingerScheduler(new StdSchedulerFactory(), pinger);
        }

        public RepeatingServerPinger CreateRepeatingServerPinger(ServerAddress serverAddress)
        {
            return new RepeatingServerPinger(serverAddress, _scheduler);
        }
    }
}