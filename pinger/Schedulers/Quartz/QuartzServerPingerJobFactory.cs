using System;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using Quartz;
using Quartz.Spi;

namespace MCLiveStatus.Pinger.Schedulers.Quartz
{
    public class QuartzServerPingerJobFactory : IJobFactory
    {
        private readonly ServerAddress _serverAddress;
        private readonly IServerPinger _serverPinger;
        private readonly Action<ServerPingResponse> _onPing;

        public QuartzServerPingerJobFactory(ServerAddress serverAddress, IServerPinger serverPinger, Action<ServerPingResponse> onPing = null)
        {
            _serverAddress = serverAddress;
            _serverPinger = serverPinger;
            _onPing = onPing;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return new QuartzServerPingerJob(_serverAddress, _serverPinger, _onPing);
        }

        public void ReturnJob(IJob job) { }
    }
}