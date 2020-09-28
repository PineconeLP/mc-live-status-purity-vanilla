using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using Quartz;

namespace MCLiveStatus.Pinger.Schedulers
{
    public class ServerPingerJob : IJob
    {
        private readonly ServerAddress _serverAddress;
        private readonly ServerPinger _serverPinger;
        private readonly Action<ServerPingResponse> _onPing;

        public ServerPingerJob(ServerAddress serverAddress, ServerPinger serverPinger, Action<ServerPingResponse> onPing = null)
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