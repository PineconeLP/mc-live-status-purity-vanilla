using System;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using Quartz;
using Quartz.Spi;

namespace MCLiveStatus.Pinger.Schedulers
{
    public class ServerPingerJobFactory : IJobFactory
    {
        private readonly ServerAddress _serverAddress;
        private readonly ServerPinger _serverPinger;
        private readonly Action<ServerPingResponse> _onPing;

        public ServerPingerJobFactory(ServerAddress serverAddress, ServerPinger serverPinger, Action<ServerPingResponse> onPing = null)
        {
            _serverAddress = serverAddress;
            _serverPinger = serverPinger;
            _onPing = onPing;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return new ServerPingerJob(_serverAddress, _serverPinger, _onPing);
        }

        public void ReturnJob(IJob job) { }
    }
}