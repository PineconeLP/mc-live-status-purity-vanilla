using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using Quartz;

namespace MCLiveStatus.Pinger.Schedulers.Quartz
{
    public class QuartzServerPingerJob : IJob
    {
        private readonly ServerAddress _serverAddress;
        private readonly IServerPinger _serverPinger;
        private readonly Action<ServerPingResponse> _onPing;

        public QuartzServerPingerJob(ServerAddress serverAddress, IServerPinger serverPinger, Action<ServerPingResponse> onPing = null)
        {
            _serverAddress = serverAddress;
            _serverPinger = serverPinger;
            _onPing = onPing;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            ServerPingResponse response = await _serverPinger.Ping(_serverAddress);

            _onPing?.Invoke(response);
        }
    }
}