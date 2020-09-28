using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Schedulers;

namespace MCLiveStatus.Pinger.Pingers
{
    public class RepeatingServerPinger
    {
        private readonly IServerPingerScheduler _scheduler;
        private readonly ServerAddress _serverAddress;

        private Func<Task> _stop;

        private bool IsRunning => _stop != null;

        public event Action<ServerPingResponse> PingCompleted;

        public RepeatingServerPinger(ServerAddress serverAddress, IServerPingerScheduler scheduler)
        {
            _serverAddress = serverAddress;
            _scheduler = scheduler;
        }

        public async Task Start()
        {
            if (!IsRunning)
            {
                _stop = await _scheduler.Start(_serverAddress, OnPingCompleted);
            }
        }

        public async Task Stop()
        {
            if (IsRunning)
            {
                await _stop();
            }
        }

        private void OnPingCompleted(ServerPingResponse response)
        {
            PingCompleted?.Invoke(response);
        }
    }
}